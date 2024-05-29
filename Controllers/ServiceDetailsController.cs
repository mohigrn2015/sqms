using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models.ViewModels;
using SQMS.Models;
using SQMS.Models.MetaModels;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;
using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SQMS.Models.ResponseModel;
using ClosedXML;


namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Branch Admin, Service Holder")]
    public class ServiceDetailsController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private readonly BLLServiceDetail dbManager = new BLLServiceDetail();
        private readonly BLLServiceType dbServiceType = new BLLServiceType();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly BLLCustomer dbCustomer = new BLLCustomer();
        private readonly BLLToken dbtoken = new BLLToken();
        private readonly BLLDailyBreak dbBreak = new BLLDailyBreak();
        private readonly Utility.ILogger<string> _logger;
        private readonly BLLServiceSubType dbServiceSubType = new BLLServiceSubType();        
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _hubContext;
        public ServiceDetailsController(notifyDisplay _notifyDisplay,IHttpContextAccessor session, IHubContext<notifyDisplay> hubContext)
        {
            _logger = new TextLogger();
            _hubContext = hubContext;
            _session = session;
        }

        // GET: ServiceDetails
        [RightPrivilegeFilter(PageIds = 800702)]
        public IActionResult Index()
        {
            SessionManager sm = new SessionManager(_session);
            int? branch_id;
            string user_id;

            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                ViewBag.userBranchId = sm.branch_id;

                if (User.IsInRole("Admin"))
                {
                    branch_id = null;
                    user_id = null;
                }
                else if (User.IsInRole("Branch Admin"))
                {
                    branch_id = sm.branch_id;
                    user_id = null;
                }
                else
                {
                    branch_id = null;
                    user_id = sm.user_id;
                }
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }           

            return View(dbManager.GetAllCurrentDate(branch_id, user_id));

        }

        [RightPrivilegeFilter(PageIds = 800701)]
        public IActionResult Create()
        {
            try
            {
                var servicetype = dbServiceType.GetAll();
                ViewBag.service_type_id = servicetype;
                //ViewBag.service_sub_type_id = new SelectList(dbServiceSubType.GetAll(), "service_sub_type_id", "service_sub_type_name");
                ViewBag.service_sub_type_id = dbServiceSubType.GetAll();
                return View();
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }

        [HttpPost]
        [AuthorizationFilter(Roles = "Branch Admin, Service Holder")]
        public IActionResult Create(VMServiceDetails model)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                DisplayManager dm = new DisplayManager();
                model.service_datetime = DateTime.Now;
                model.customer_name = model.contact_no;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.counter_id = sm.counter_id;
                model.starting_time = model.start_time.TimeOfDay;
                //model.start_DateTime = model.start_time.ToString("yyyy-MM-dd HH:mm:ss");   //Convert.ToDateTime(model.start_time);

                //if (ModelState.IsValid)
                //{
                    dbManager.Create(model);
                //}
                //else
                //{
                //    return Ok(new { Success = false, Message = "Model state is not valid" });
                //}
                return Ok(new { Success = true, Message = "Customer Service Information updated on Server" });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }
        [HttpPost]
        [AuthorizationFilter(Roles = "Branch Admin, Service Holder")]
        public IActionResult Done(VMServiceDetails model)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                DisplayManager dm = new DisplayManager();
                int branchId = sm.branch_id;
                string counter_no = sm.counter_no;
                model.service_datetime = DateTime.Now;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.counter_id = sm.counter_id;
                if (ModelState.IsValid)
                {
                    dbManager.Create(model);
                }
                else
                {
                    return Ok(new { Success = false, Message = "Model state is not valid" });
                }
                return Ok(new { Success = true, Message = "Customer Service Information updated on Server" });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }


        [HttpPost]
        //[ModelValidationFilter]
        //[CustomExceptionFilter]
        [AuthorizationFilter(Roles = "Branch Admin, Service Holder")]
        public IActionResult AddService(VMServiceDetails model)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                DisplayManager dm = new DisplayManager();
                model.service_datetime = DateTime.Now;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.counter_id = sm.counter_id;
                if (ModelState.IsValid)
                {
                    dbManager.AddService(model);
                }
                return Ok(new { Success = true, Message = "Customer Service Information updated on Server" });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }

        [HttpPost]
        //[Route("NewTokenNo")]
        public async Task<IActionResult> NewTokenNo()
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            SessionManager sm = new SessionManager(_session);
            int branchId = sm.branch_id;
            int counterid = sm.counter_id;
            string counter_no = sm.counter_no;
            string user_id = sm.user_id;
            CommonResponseModel common = new CommonResponseModel();
            CommonLogModel commonLogModel = new CommonLogModel();
            CommonLogModelv2 commonLogModelv2 = new CommonLogModelv2();

            try
            {
                var serviceList = dbManager.GetNewToken(branchId, counterid, user_id, out long token_id, out string token_prefix, out int token_no, out string contact_no
                    , out string service_type, out DateTime start_time, out string customer_name, out string address, out DateTime generate_time, out int is_break);
                commonLogModel.model = serviceList;
                if (serviceList.Count > 0)
                {
                    var services = new SelectList(serviceList, "service_sub_type_id", "service_sub_type_name");
                    var servicesTAT = new SelectList(serviceList, "service_sub_type_id", "tat_warning_time");
                    commonLogModelv2.model = services;


                    var customer = new
                    {
                        token = token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0'),
                        start_time = start_time.ToString("hh:mm:ss tt"),
                        tokenid = token_id,
                        serviceType = service_type,
                        mobile_no = contact_no,
                        user_id = user_id,
                        generate_time = generate_time.ToString("hh:mm:ss tt"),
                        call_time = start_time.ToString("hh:mm:ss tt"),
                        IsBreak = is_break,
                        waitingtime = start_time.Subtract(generate_time).ToString(),
                        service_type_id = (serviceList.Count > 0 ? serviceList.FirstOrDefault().service_type_id : 0),
                        customer_name,
                        address
                    };

                    commonLogModel.message = customer;
                    string voiceToken = Regex.Replace(customer.token, ".{1}", "$0, ");
                   await notifyDisplay.SendMessages(branchId, counter_no, voiceToken, false, true, false, false);
                    await notifyDisplay.CounterStatusChanged(sm.branch_id);
                    return Ok(new { success = true, message = customer, services = services, TAT = servicesTAT });
                } 
                else 
                {
                    await notifyDisplay.SendMessages(branchId, "", "", false, true, false, false);
                    await notifyDisplay.CounterStatusChanged(sm.branch_id);
                    return Ok(new { success = false, message = "No token for new service!", isBreak = is_break });
                }

            }
            catch (Exception ex)
            {
                common = new CommonResponseModel()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(common);
                //if (ex.Message.ToUpper() == "invalid operation on null data".ToUpper())
                //{
                //    await notifyDisplay.SendMessages(branchId, "", "", false, true, false, false);
                //    await notifyDisplay.CounterStatusChanged(sm.branch_id);
                //    return Ok(new { success = false, sessage = "No token for new service!", IsBreak = 0 });
                //} 
                //else
                //    return Ok(new { success = false, ex.Message });
            }
            finally
            {
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite("NewTokenNo: model: " + JsonConvert.SerializeObject(commonLogModel)  + " common: " + JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CallManualTokenNo(string token_no_string)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branchId = sm.branch_id;
                int counterid = sm.counter_id;
                string counter_no = sm.counter_no;
                string user_id = sm.user_id;


                var serviceList = dbManager.CallManualToken(branchId, counterid, user_id, token_no_string, out long token_id, out string contact_no, out string service_type
                    , out DateTime start_time, out string customer_name, out string address);

                if (serviceList.Count > 0)
                {
                    var services = new SelectList(serviceList, "service_sub_type_id", "service_sub_type_name");

                    var customer = new
                    {
                        token = token_no_string,
                        start_time = start_time.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                        tokenid = token_id,
                        serviceType = service_type,
                        mobile_no = contact_no,
                        customer_name,
                        address
                    };
                    //NotifyDisplay.SendMessages(branchId, counter_no, token_no.ToString());
                    return Ok(new { Success = true, Message = customer, Services = services });
                }
                else
                {
                   await notifyDisplay.SendMessages(branchId, counter_no, "", false, false, false, false);
                    return Ok(new { Success = false, Message = "No token for new service!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CancelTokenNo(long tokenID)
        {
            try
            {
                int token_no = dbManager.CancelToken(tokenID);
                return Ok(new { Success = true, Message = "Service Canceled for Token No #" + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0') });
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }

        [HttpPost]
        public IActionResult Cancel(long tokenID)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                DisplayManager dm = new DisplayManager();
                int branchId = sm.branch_id;
                string counter_no = sm.counter_no;
                int token_no = dbManager.CancelToken(tokenID);
                return Ok(new { Success = true, Message = "Service Canceled for Token No #" + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0') });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(long token_id, string counter_no)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                SessionManager sm = new SessionManager(_session);

                int counter_id = dbManager.Transfer(sm.branch_id, counter_no, token_id); //Added a return value
                await notifyDisplay.CallToken(counter_id); //Called token notfication with new value

                return Ok(new { Success = true, Message = "Service transfered to counter #" + counter_no + ", customer must wait for calling" });

            }
            catch (MySqlException ex) when (ex.Number == 20001)
            {
                return Ok(new { Success = false, Message = "Counter not found" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }

        public IActionResult GetCustomerInformation(long token_id, string contact_no)
        {
            try
            {
                tblCustomer customerDetails = dbCustomer.GetAll().Where(a => a.contact_no == contact_no).FirstOrDefault();

                if (customerDetails != null)
                {
                    List<tblServiceDetail> previousHistoryList = dbManager.GetByCustomerID(customerDetails.customer_id);
                    List<VMServiceDetails> customerlist = new List<VMServiceDetails>();

                    foreach (tblServiceDetail item in previousHistoryList)
                    {
                        VMServiceDetails VMServiceDetails = new VMServiceDetails()
                        {
                            issues = item.issues,
                            solutions = item.solutions,
                            service_datetime = item.service_datetime
                        };
                        customerlist.Add(VMServiceDetails);
                    }
                    return Ok(new { Success = true, Message = customerlist, customerDetails });
                }
                else
                {
                    return Ok(new { Success = false, Message = "", customerDetails = "" });
                }
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }        
        }

        //[AuthorizationFilter(Roles = "Service Holder")]
        public IActionResult ServiceList()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                List<VMDashboardBranchService> serviceList = new BLLDashboard().GetBranchServiceList(sm.branch_id);
                List<VMDashboardBranchToken> tokenList = new BLLDashboard().GetBranchTokenList(sm.branch_id);
                ViewBag.tokenList = tokenList;
                return PartialView(serviceList);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public IActionResult CounterServiceList()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                List<VMDashboardCounterService> serviceList = new BLLDashboard().GetCounterServiceList(sm.counter_id, out string serving_time);
                List<VMDashboardCounterToken> tokenList = new BLLDashboard().GetCounterTokenList(sm.counter_id);
                ViewBag.tokenList = tokenList;

                List<Models.tblTokenQueue> tokens = new BLLToken().GetBy(from_date: DateTime.Now, to_date: DateTime.Now, branch_id: sm.branch_id);

                AspNetUserLogin login = new BLLAspNetUser().GetLoginInfo(sm.user_id);
                ViewBag.UserName = sm.user_name;
                ViewBag.CounterNo = sm.counter_no;
                ViewBag.LoginTime = login.login_time.ToString("hh:mm:ss tt");
                ViewBag.ServingTime = serving_time;
                if (tokens.Count > 0)
                {
                    ViewBag.TotalPending = tokens.Where(w => w.service_status_id == 1 || w.service_status_id == 2).Count();
                    ViewBag.TotalServed = tokens.Where(w => w.service_status_id == 5 && w.counter_id == sm.counter_id).Count();
                    var counterServedTokens = tokens.Where(w => w.service_status_id == 5 && w.counter_id == sm.counter_id).ToList();
                    var hw = counterServedTokens.Max(m => m.CallTime - m.service_date);
                    ViewBag.HighestWaiting = (hw.HasValue ? string.Format("{0:hh\\:mm\\:ss}", (hw.Value)) : "00:00:00");
                    var aw = CalculateAverageWaitingTime(counterServedTokens);
                    ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", aw);
                }
                else
                {
                    ViewBag.TotalPending = 0;
                    ViewBag.TotalServed = 0;
                    ViewBag.HighestWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                    ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                }

                return PartialView(serviceList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }            
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public IActionResult UserServiceList()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                List<VMDashboardUserService> serviceList = new BLLDashboard().GetUserServiceList(sm.user_id, out string serving_time);
                List<VMDashboardUserToken> tokenList = new BLLDashboard().GetUserTokenList(sm.user_id);
                List<VMDashboardUserServiceDetail> serviceDetailList = new BLLDashboard().GetUserServiceDetailList(sm.user_id);
                ViewBag.tokenList = tokenList;
                ViewBag.serviceDetailList = serviceDetailList;

                List<Models.tblTokenQueue> tokens = new BLLToken().GetBy(from_date: DateTime.Now, to_date: DateTime.Now, branch_id: sm.branch_id);

                AspNetUserLogin login = new BLLAspNetUser().GetLoginInfo(sm.user_id);
                ViewBag.UserName = sm.user_name;
                ViewBag.CounterNo = sm.counter_no;
                ViewBag.LoginTime = login.login_time.ToString("hh:mm:ss tt");
                ViewBag.ServingTime = serving_time;
                if (tokens.Count > 0)
                {
                    ViewBag.TotalPending = tokens.Where(w => w.service_status_id == 1 || w.service_status_id == 2).Count();
                    ViewBag.TotalServed = tokens.Where(w => w.service_status_id == 5 && w.user_id == sm.user_id).Count();
                    var counterServedTokens = tokens.Where(w => w.service_status_id == 5 && w.user_id == sm.user_id).ToList();
                    var hw = counterServedTokens.Max(m => m.CallTime - m.service_date);
                    ViewBag.HighestWaiting = (hw.HasValue ? string.Format("{0:hh\\:mm\\:ss}", (hw.Value)) : "00:00:00");
                    var aw = CalculateAverageWaitingTime(counterServedTokens);
                    ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", aw);
                }
                else
                {
                    ViewBag.TotalPending = 0;
                    ViewBag.TotalServed = 0;
                    ViewBag.HighestWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                    ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                }

                return PartialView(serviceList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandlerV2");
            }            
        }

        private TimeSpan CalculateAverageWaitingTime(List<Models.tblTokenQueue> tokens)
        {
            try
            {
                if (tokens.Count > 0)
                {
                    double doubleAverageTicks = tokens.Average(timeSpan => timeSpan.waitingtimeToTimeStamp.Ticks);
                    long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

                    return new TimeSpan(longAverageTicks);
                }
                else return default(TimeSpan);

            }
            catch (Exception ex)
            {
                return default(TimeSpan);
            }
        }
    }
}
