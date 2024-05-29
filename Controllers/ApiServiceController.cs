using DocumentFormat.OpenXml.Bibliography;
using Irony.Parsing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.ReqModel;
using SQMS.Models.RequestModel;
using SQMS.Models.ResponseModel;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;

namespace SQMS.Controllers
{
    [Route("ApiService")]
    [ApiController]
    public class ApiServiceController : ControllerBase
    {
        private readonly BLLDashboard db = new BLLDashboard();
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        private readonly ILogInFile _logger;
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _hubContext;
        public ApiServiceController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor session, IHubContext<notifyDisplay> hubContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = new TextLogger();
            _session = session;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("GetServiceList")]
        public IActionResult GetServiceList([FromBody] CommonReqModel model)
        {
            string responseJson = String.Empty;
            try
            {
                ApiManager.ValidUserBySecurityToken(model.securityToken);
                BLLServiceType dbServiceType = new BLLServiceType();

                var serviceList = dbServiceType.GetAll().ToList();

                var result = new { success = true, serviceList };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var resultException = new { success = true, ex.Message };

                return Ok(resultException);
            }
            finally
            {
                var requestJson = new { success = true, model.securityToken };
                ApiManager.Loggin(model.securityToken, "GetServiceList", requestJson.ToString(), responseJson);
            }
        }

        [HttpGet]
        [Route("HasService")]
        public IActionResult HasService()
        {
            try
            {
                return Ok(new { success = true, message = "Service is running" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetDBDate")]
        public IActionResult GetDBDate()
        {
            try
            {
                DateTime dbdate = new DAL.MySQLManager().CallStoredProcedure_DBDate();
                return Ok(new { success = true, dbdate = dbdate.ToString("hh:mm:ss tt") });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GetAllBranches")]
        public IActionResult GetAllBranches()
        {
            try
            {
                BLLBranch dbBranch = new BLLBranch();

                var List = dbBranch.GetAllBranch().ToList();

                return Ok(new { success = true, branchList = List });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GetTokenList")]
        public IActionResult GetTokenList(CommonReqModel model)
        {

            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                BLLToken dbToken = new BLLToken();

                var tokenList = dbToken.GetByBranchId(branchUser.branch_id);

                return Ok(new { success = true, tokenList });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            finally
            {
                ApiManager.Loggin(model.securityToken, "GetTokenList", model.securityToken, model.securityToken);
            }
        }


        [HttpPost]
        [Route("GenerateNewToken")]
        public async Task<IActionResult> GenerateNewToken(NewTokenReqModel model)
        {
            string responseJson = String.Empty;
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            LoyaltyResponse loyaltyResponse = new LoyaltyResponse();
            LMSApiCall lMSApiCall = new LMSApiCall();
            CustomerCategoryReqModel reqModel = new CustomerCategoryReqModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                string tokenPrfx = string.Empty;
                string msisdn = string.Empty;
                if (!String.IsNullOrEmpty(model.mobile))
                {
                    if(model.mobile.Substring(0,2) != "88")
                    {
                        msisdn = "88"+model.mobile;
                    }
                    else
                    {
                        msisdn = model.mobile;
                    }

                    reqModel = new CustomerCategoryReqModel()
                    {
                        channel = StaticConfigValue.GetLMSChannel(),
                        msisdn = msisdn,
                        transactionID = StaticConfigValue.GetLMSTransactionId()
                    };
                    try
                    {
                        loyaltyResponse = await lMSApiCall.CallLMSapi(reqModel);
                    }
                    catch
                    {
                        tokenPrfx = StaticConfigValue.GetDefault_token_prfx();
                    }
                }               
                
                if(loyaltyResponse != null)
                {
                    if(loyaltyResponse.LoyaltyProfileInfo != null)
                    {
                        if(loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "PLATINUM" || loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "SIGNATURE" || loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "GOLD")
                        {
                            tokenPrfx = "P";
                        }
                        else
                        {
                            tokenPrfx = StaticConfigValue.GetDefault_token_prfx();
                        }
                    }
                    else
                    {
                        tokenPrfx = StaticConfigValue.GetDefault_token_prfx();
                    }
                }
                else
                {
                    tokenPrfx = StaticConfigValue.GetDefault_token_prfx();
                }

                tblTokenQueue tokenObj = new tblTokenQueue()
                {
                    branch_id = branchUser.branch_id,
                    contact_no = model.mobile,
                    service_type_id = model.service_type_id,
                    token_prefix = tokenPrfx,

                };
                await new BLLToken(_hubContext).Create(tokenObj);
                
                await notifyDisplay.SendMessages(branchUser.branch_id, "", "", true, false, false, false);

                string token_id = tokenObj.token_id.ToString();
                string token_no = tokenObj.token_no_formated;
                string date = Convert.ToString(DateTime.Now);

                return Ok(new { success = true, tokenNo = token_no });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GenerateNewToken: model: " + JsonConvert.SerializeObject(model)+ " responseModel: " + JsonConvert.SerializeObject(loyaltyResponse) + " common: "+ JsonConvert.SerializeObject(model.securityToken));

                ApiManager.Loggin(model.securityToken, "GenerateNewToken", "", responseJson);
            }
        }

        [HttpPost]
        [Route("SendSMS")]
        public async Task<IActionResult> SendSMS(SendSMSRequestModel model)
        {
            BLLSMS bLLSMS = new BLLSMS();
            string responseJson = String.Empty;
            SMSDataResponseModel smsMOdel  = new SMSDataResponseModel();
            SMSApiCall sMSApiCall = new SMSApiCall();
            try
            {
                ApiManager.ValidUserBySecurityToken(model.securityToken);
                BLLToken tokenManager = new BLLToken();                

                if (ApplicationSetting.isMsgBn)
                {
                    smsMOdel.msisdn = model.mobile;
                    smsMOdel.token_no = model.tokenNo;
                    smsMOdel.is_bn = 1;
                    smsMOdel.message = String.Format(ApplicationSetting.msgText_Bn,model.tokenNo);
                    await sMSApiCall.SendSms(smsMOdel);
                }
                else
                {
                    smsMOdel.msisdn = model.mobile;
                    smsMOdel.token_no = model.tokenNo;
                    smsMOdel.is_bn = 0;
                    smsMOdel.message = String.Format(ApplicationSetting.msgText,model.tokenNo);
                    await sMSApiCall.SendSms(smsMOdel);
                }
                return Ok(new { success = true, message = "SMS Sent Succesfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            finally
            {
                string requestJson = JsonConvert.SerializeObject(new { model.mobile, model.tokenNo, model.securityToken });
                ApiManager.Loggin(model.securityToken, "SendSMS", requestJson, responseJson);
            }
        }

        [HttpPost]
        [Route("GetDisplayInfo")]
        public IActionResult GetDisplayInfo([FromBody] CommonReqModel model)
        {
            string responseJson = String.Empty;
            DisplayInfoResponseModel responseModel = new DisplayInfoResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                DisplayManager dm = new DisplayManager();

                var tokenInProgress = dm.GetInProgressTokenList(branchUser.branch_id);

                string nextToken = dm.GetNextTokens(branchUser.branch_id);

                BLLPlayList playListManager = new BLLPlayList();
                var playLists = playListManager.GetByBranchId(branchUser.branch_id).FirstOrDefault();

                BLLDisplayFooter footerManager = new BLLDisplayFooter(_webHostEnvironment);
                var displayFooter = footerManager.GetByBranchId(branchUser.branch_id);

                responseModel = new DisplayInfoResponseModel()
                {
                    success = true,
                    tokenInProgress = tokenInProgress,
                    nextTokens = nextToken,
                    playlist_id = playLists.playlist_id,
                    displayFooter = displayFooter
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common); 
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GetDisplayInfo: model: " + JsonConvert.SerializeObject(model) + " responseModel: " + JsonConvert.SerializeObject(responseModel) + " common: " + JsonConvert.SerializeObject(common));

                string requestJson = JsonConvert.SerializeObject(new { model.securityToken });
                ApiManager.Loggin(model.securityToken, "GetDisplayInfo", requestJson, responseJson);
            }
        }

        [HttpPost]
        [Route("GetCounterDisplayInfo")]
        public IActionResult GetCounterDisplayInfo( [FromBody] APIServiceCommonRequestModel model)
        {
            string responseJson = String.Empty;
            CounterDisplayInfoResponseModel counterDisplay = new CounterDisplayInfoResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                DisplayManager dm = new DisplayManager();

                var tokenInProgress = dm.GetInProgressTokenList(branchUser.branch_id);

                string nextToken = dm.GetNextTokens(branchUser.branch_id);

                counterDisplay = new CounterDisplayInfoResponseModel()
                {
                    success = true,
                    tokenInProgress = tokenInProgress,
                    nextTokens = nextToken
                };

                return Ok(counterDisplay);
            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common);
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GetCounterDisplayInfo: model: " + JsonConvert.SerializeObject(model) + " responseModel: " + JsonConvert.SerializeObject(counterDisplay) + " common: " + JsonConvert.SerializeObject(common));


                string requestJson = JsonConvert.SerializeObject(new { model.securityToken });
                ApiManager.Loggin(model.securityToken, "GetCounterDisplayInfo", requestJson, responseJson);
            }
        }

        [HttpPost]
        [Route("GetNextTokens")]
        public IActionResult GetNextTokens([FromBody] APIServiceCommonRequestModel model)
        {
            string responseJson = String.Empty;
            NextTokenResponseModel responseModel = new NextTokenResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                DisplayManager dm = new DisplayManager();

                string nextToken = dm.GetNextTokens(branchUser.branch_id);

                responseModel = new NextTokenResponseModel()
                {
                    success = true,
                    nextTokens = nextToken
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common);
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GetNextTokens: model: " + JsonConvert.SerializeObject(model) + " responseModel: " + JsonConvert.SerializeObject(responseModel) + " common: " + JsonConvert.SerializeObject(common));

                string requestJson = JsonConvert.SerializeObject(new { model.securityToken });
                ApiManager.Loggin(model.securityToken, "GetNextTokens", requestJson, responseJson);
            }
        }

        [HttpPost]
        [Route("GetPlayList")]
        public IActionResult GetPlayList([FromBody] APIServiceCommonRequestModel model)
        {
            string responseJson = String.Empty;
            PlayListResponseModel responseModel = new PlayListResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                BLLPlayList playListManager = new BLLPlayList();

                var playLists = playListManager.GetByBranchId(branchUser.branch_id);

                responseModel = new PlayListResponseModel() 
                {
                    success = true,
                    playLists = playLists 
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common);
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GetPlayList: model: " + JsonConvert.SerializeObject(model) + " responseModel: " + JsonConvert.SerializeObject(responseModel) + " common: " + JsonConvert.SerializeObject(common));

                string requestJson = JsonConvert.SerializeObject(new { model.securityToken });
                ApiManager.Loggin(model.securityToken, "GetPlayList", requestJson, responseJson);
            }

        }

        [HttpPost]
        [Route("GetDisplayFooter")]
        public IActionResult GetDisplayFooter([FromBody] APIServiceCommonRequestModel model)
        {
            string responseJson = String.Empty;
            DisplayResponseModel responseModel = new DisplayResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(model.securityToken);

                VMBranchLogin branchUser = new BLLBranchUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (branchUser == null) throw new Exception("User is not assigned in any branch");

                BLLDisplayFooter footerManager = new BLLDisplayFooter(_webHostEnvironment);

                var displayFooter = footerManager.GetByBranchId(branchUser.branch_id);

                responseModel = new DisplayResponseModel()
                {
                    success = true,
                    displayFooter = displayFooter
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common);
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("GetDisplayFooter: model: " + JsonConvert.SerializeObject(model) + " responseModel: " + JsonConvert.SerializeObject(responseModel) + " common: " + JsonConvert.SerializeObject(common));

                string requestJson = JsonConvert.SerializeObject(new { model.securityToken });
                ApiManager.Loggin(model.securityToken, "GetDisplayFooter", requestJson, responseJson);
            }
        }

        [AuthorizationFilter(Roles = "Admin")]
        [HttpPost]
        [Route("GetAdminDashboard")]
        public IActionResult GetAdminDashboard(int id = 0)
        {
            try
            {
                List<VMDashboardBranchAdminServicesTokens> ServicesTokens = new List<VMDashboardBranchAdminServicesTokens>();
                List<VMDashboardBranchAdminServicesWaitings> ServicesWaitings = new List<VMDashboardBranchAdminServicesWaitings>();
                var dashboardData = db.GetBranchAdminDashboard(id, ServicesTokens, ServicesWaitings);
                var BranchTokenList = db.GetBranchTokenList(id);
                var BranchServiceList = db.GetBranchServiceList(id);
                var BranchServiceDetailList = db.GetBranchServiceDetailList(id);
                return Ok(new { success = true, data = dashboardData, servicesTokens = ServicesTokens, servicesWaitings = ServicesWaitings, branchTokenList = BranchTokenList, branchServiceList = BranchServiceList, branchServiceDetailList = BranchServiceDetailList });
            }
            catch (Exception)
            {

                throw;
            }

        }

        [AuthorizationFilter(Roles = "Branch Admin")]
        [HttpPost]
        [Route("GetBranchAdminDashboard")]
        public IActionResult GetBranchAdminDashboard()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                List<VMDashboardBranchAdminServicesTokens> ServicesTokens = new List<VMDashboardBranchAdminServicesTokens>();
                List<VMDashboardBranchAdminServicesWaitings> ServicesWaitings = new List<VMDashboardBranchAdminServicesWaitings>();
                var dashboardData = db.GetBranchAdminDashboard(sm.branch_id, ServicesTokens, ServicesWaitings);
                var BranchTokenList = db.GetBranchTokenList(sm.branch_id);
                var BranchServiceList = db.GetBranchServiceList(sm.branch_id);
                var BranchServiceDetailList = db.GetBranchServiceDetailList(sm.branch_id);

                return Ok(new { success = true, data = dashboardData, servicesTokens = ServicesTokens, servicesWaitings = ServicesWaitings, branchTokenList = BranchTokenList, branchServiceList = BranchServiceList, branchServiceDetailList = BranchServiceDetailList });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("WriteLog")]
        public IActionResult WriteLog(VMLogWrite model)
        {
            try
            {
                _logger.LogWrite(JsonConvert.SerializeObject(model));

                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return Ok(new { success = false });
            }
        }
    }
}
