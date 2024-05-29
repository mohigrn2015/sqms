using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.RequestModel;
using SQMS.Models.ResponseModel;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;

namespace SQMS.Controllers
{
    //ReportingSerivceLib.PrintManager objPrintManager = new PrintManager();

    //IList<ParameterValue> parameters = new List<ParameterValue>();

    public class TokenQueuesController : Controller
    {
        private readonly BLLToken dbManager = new BLLToken();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly BLLStatus dbStatus = new BLLStatus();
        private readonly BLLServiceType dbServiceType = new BLLServiceType();
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _context;
        public TokenQueuesController(notifyDisplay notifyDisplay, IHttpContextAccessor session, IHubContext<notifyDisplay> context)
        {
            _context = context;
            _session = session;
        }

        // GET: TokenQueues
        [AuthorizationFilter(Roles = "Admin, Branch Admin,Token Generator")]
        [RightPrivilegeFilter(PageIds = 800722)]
        public IActionResult Index(String branch_name, string counter_no)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                ViewBag.service_status = dbStatus.GetAll();


                //tblTokenQueue tokenObj = new tblTokenQueue();
                ViewBag.userBranchId = sm.branch_id;
                //var tblTokenQueues = db.tblTokenQueues.Include(i => i.tblCounter).Include(i => i.tblServiceDetails);
                //string token_id = tokenObj.token_id.ToString();
                return View(dbManager.GetByBranchId(sm.branch_id));
                //return View(await tblTokenQueues.OrderByDescending(o=>o.token_id).ToListAsync());
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: TokenQueues
        [AuthorizationFilter]
        public IActionResult Skipped()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();

                ViewBag.userBranchId = sm.branch_id;

                int? branch_id;
                string user_id;
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

                return View(dbManager.GetSkipped(branch_id, user_id));
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Branch Admin,Service Holder")]
        public async Task<IActionResult> ReInitiate(long token_id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branchId = sm.branch_id;

                string counter_no = sm.counter_no;

                dbManager.ReInitiate(token_id);
                await notifyDisplay.SendMessages(branchId, "", "", true, false, false, false);
                return Ok(new { Success = true, Message = "Successfully Token Re-initiated" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }
        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Service Holder")]
        public async Task<IActionResult> AssignToMe(long token_id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branchId = sm.branch_id;
                int counter_id = sm.counter_id;
                string counter_no = sm.counter_no;

                dbManager.AssignToMe(token_id, counter_id);
                await notifyDisplay.SendMessages(branchId, "", "", true, false, false, false);
                return Ok(new { Success = true, Message = "Successfully Token Assigned To Me" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }

        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Branch Admin")]
        public async Task<IActionResult> AssignToCounter(long token_id, string counter_no)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branchId = sm.branch_id;

                tblCounter counter = new BLLCounters().GetAllCounter().Where(c => c.counter_no.ToLower() == counter_no.ToLower() && c.branch_id == branchId).FirstOrDefault();
                if (counter == null)
                {
                    return Ok(new { Success = false, Message = "Counter not found" });
                }

                VMBranchCounterStatus counterStatus = new BLLBranch().GetCounterCurrentStatus(branchId, counter.counter_id).FirstOrDefault();
                if (counterStatus == null)
                {
                    return Ok(new { Success = false, Message = "No user is available in this counter" });
                }

                if (counterStatus.login_time.HasValue == false)
                {
                    return Ok(new { Success = false, Message = "No user is available in this counter" });
                }

                dbManager.AssignToMe(token_id, counter.counter_id);
                await notifyDisplay.SendMessages(branchId, "", "", true, false, false, false);
                return Json(new { Success = true, Message = "Successfully token assigned to counter" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }
        // GET: TokenQueues/Details/5
        //[AuthorizationFilter(Roles = "Admin, Branch Admin")]
        //public async Task<IActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblTokenQueue);
        //}

        [AuthorizationFilter(Roles = "Branch Admin, Token Generator")]
        [RightPrivilegeFilter(PageIds = 800723)]
        public IActionResult Create()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                var serviceList = dbServiceType.GetAll();
                ViewBag.ServiceTypeList = serviceList;
                return View();
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        //[AuthorizationFilter(Roles = "Branch Admin, Token Generator")]
        //[HttpPost]
        //public JsonResult Create(string mobile, string service)
        //{
        //    try
        //    {
        //        SessionManager sm = new SessionManager(_session);
        //        int branchId = sm.branch_id;

        //        DisplayManager dm = new DisplayManager();



        //        string subString = "Generated Token No is  #";
        //        var maxToken = dbManager.GetAll()
        //            .Where(w =>
        //                w.branch_id == branchId
        //                && /*w.service_date == DateTime.Now*/
        //                DbFunctions.TruncateTime(w.service_date) == DbFunctions.TruncateTime(DateTime.Now)
        //            );

        //        int tokenNo = 0;

        //        if (maxToken.Any())
        //        {
        //            tokenNo = maxToken.Max(m => m.token_no);
        //        }


        //        tblTokenQueue tokenObj = new tblTokenQueue();
        //        tokenObj.contact_no = mobile;
        //        tokenObj.service_date = DateTime.Now;

        //        tokenObj.token_no = tokenNo + 1;



        //        //dm.SendSms(tokenNo);
        //        //db.SaveChanges();



        //        tokenObj.service_status_id = 1;

        //        tokenObj.branch_id = branchId;
        //        tokenObj.service_type_id = Convert.ToInt32(service);
        //        dbManager.Create(mobile,service);


        //        //if (!String.IsNullOrEmpty(sm.branch_static_ip))
        //        //    dm.CreateTextFile(sm.branch_id, sm.branch_static_ip);

        //        string message = subString + tokenObj.token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
        //        NotifyDisplay.SendMessages(branchId, "null", "null");

        //        string token_id = tokenObj.token_id.ToString();
        //        string token_no = tokenObj.token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
        //        string date =Convert.ToString(DateTime.Now);
        //        //DisplayManager dm = new DisplayManager();


        //        return Json(new { Success = true, Message = message, tokenId = token_id, tokenNo = token_no, Date = date,msisdn=mobile }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Problem with Token Create, Please Try Again!" }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        [AuthorizationFilter(Roles = "Branch Admin, Token Generator")]
        [HttpPost]
        public async Task<IActionResult> Create(string mobile, int service)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            BLLToken dbManager = new BLLToken(_context);
            string message = String.Empty;
            string token_id = String.Empty;
            string token_no = String.Empty;
            LoyaltyResponse loyaltyResponse = new LoyaltyResponse();
            LMSApiCall lMSApiCall = new LMSApiCall();
            CustomerCategoryReqModel reqModel = new CustomerCategoryReqModel();
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branchId = sm.branch_id;

                string tokenPrfx = string.Empty;
                string msisdn = string.Empty;
                if (!String.IsNullOrEmpty(mobile))
                {
                    if (mobile.Substring(0, 2) != "88")
                    {
                        msisdn = "88" + mobile;
                    }
                    else
                    {
                        msisdn = mobile;
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

                if (loyaltyResponse != null)
                {
                    if (loyaltyResponse.LoyaltyProfileInfo != null)
                    {
                        if (loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "PLATINUM" || loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "SIGNATURE" || loyaltyResponse.LoyaltyProfileInfo.CurrentTierLevel.ToUpper() == "GOLD")
                        {
                            tokenPrfx = "P";
                        }
                        else
                        {
                            tokenPrfx = StaticConfigValue.GetDefault_token_prfx();
                        }
                    }
                }


                //DisplayManager dm = new DisplayManager();
                tblTokenQueue tokenObj = new tblTokenQueue()
                {
                    branch_id = branchId,
                    contact_no = mobile,
                    service_type_id = service,
                    token_prefix = tokenPrfx
                };
                await dbManager.Create(tokenObj);
                if (tokenObj.token_id != 0)
                {
                    string subString = "Token No is  #";
                    token_id = tokenObj.token_id.ToString();
                    token_no = tokenObj.token_no_formated;
                    message = subString + tokenObj.token_no_formated;

                    await notifyDisplay.SendMessages(branchId, "", "", true, false, false, false);
                }
                return Ok(new { Success = true, Message = message, tokenId = token_id, tokenNo = token_no, msisdn = mobile });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }



        //[AuthorizationFilter(Roles = "Admin, Branch Admin")]
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.branch_id = new SelectList(db.tblBranches, "branch_id", "branch_name", tblTokenQueue.branch_id);
        //    ViewBag.service_status_id = new SelectList(db.tblServiceStatus, "service_status_id", "service_status", tblTokenQueue.service_status_id);
        //    return View(tblTokenQueue);
        //}

        // POST: TokenQueues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Branch Admin")]
        //public async Task<IActionResult> Edit([Bind(Include = "token_id,branch_id,token_no,service_date,service_status_id,contact_no")] tblTokenQueue tblTokenQueue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblTokenQueue).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.branch_id = new SelectList(db.tblBranches, "branch_id", "branch_name", tblTokenQueue.branch_id);
        //    ViewBag.service_status_id = new SelectList(db.tblServiceStatus, "service_status_id", "service_status", tblTokenQueue.service_status_id);
        //    return View(tblTokenQueue);
        //}

        // GET: TokenQueues/Delete/5
        //[AuthorizationFilter(Roles = "Admin, Branch Admin")]
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblTokenQueue);
        //}

        //// POST: TokenQueues/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Branch Admin")]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    db.tblTokenQueues.Remove(tblTokenQueue);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        //public IActionResult GetList(DateTime date)
        //{
        //    DisplayManager dm = new DisplayManager();

        //    var v = dm.DateListToken((DateTime)date).ToList();
        //    List<VMTokenQueue> dateList = v.Select(s => new VMTokenQueue
        //    {
        //        //Branch_Name = s.Branch_Name,
        //        //Counter_Name = s.Counter_Name,
        //        //UserName = s.UserName,
        //        //start_time = s.start_time,
        //        //end_time = s.end_time,
        //        //customer_name = s.customer_name,
        //        //issues = s.issues,
        //        //solutions = s.solutions
        //    }).ToList();

        //    return PartialView(dateList);
        //}


        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Branch Admin, Token Generator")]
        public IActionResult SMSSend(string mobileNo, string tokenNo)
        {
            try
            {
                BLLToken tokenManager = new BLLToken();

                tokenManager.SendSMS(mobileNo, tokenNo);
                return Ok(new { Success = true, Message = "SMS Saved Succesfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }


        }

        //private void PrintInvoice(string invoice)
        //{
        //     //connectionString= 
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand("spINSERT_dbo_SmsLog", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@token_id", invoice);


        //    con.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    //  frmInvoiceReportPrint ff = new frmInvoiceReportPrint(dr);
        //    // ff.ShowDialog();


        //    LocalReport report = new LocalReport();
        //    string exeFolder = System.Windows.Forms.Application.StartupPath;
        //    //report.ReportPath = Path.Combine(exeFolder, @"Report\InvoicePrintRpt.rdlc");
        //    report.ReportPath = Path.Combine(exeFolder, @"Report\AMBBookingInvoice.rdlc");
        //    report.DataSources.Add(new ReportDataSource("spINSERT_dbo_SmsLog", dr));
        //    TokenPrint objprint = new TokenPrint();
        //    objprint.Export(report);
        //    objprint.Print();

        //    dr.Close();
        //    con.Close();
        //}

        public IActionResult Print(int tokenNo)
        {
            try
            {
                string TokenText = "Token #";
                string token_no = TokenText + tokenNo.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
                string date = Convert.ToString(DateTime.Now);

                return Ok(new { Success = true, Message = token_no, Date = date });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = ex.Message.ToString(), Date = DateTime.Now });
            }
        }
    }
}
