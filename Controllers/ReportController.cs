using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mysqlx.Notice;
using MySqlX.XDevAPI;
using SQMS.Models.ReportModels;
using SQMS.Models.ViewModels;
using SQMS.Models;
using SQMS.Utility;
using Mysqlx;
using SQMS.BLL;
using SQMS.DAL;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Branch Admin, Service Holder")]
    public class ReportController : Controller
    {
        private BLL.BLLBranch dbBranch = new BLL.BLLBranch();
        private readonly IHttpContextAccessor _session;
        public ReportController(IHttpContextAccessor session)
        {
            _session = session;
        }
        public IActionResult Index()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                int branch_id = sm.branch_id;
                List<tblBranch> branchList = new List<tblBranch>();
                branchList = dbBranch.GetAllBranch();
                ViewBag.branchList = branchList;
                ViewBag.branch_id = new SelectList(branchList, "branch_id", "branch_name", branch_id);
                List<VMServiceType> serviceList = new BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
                // ViewBag.serviceList = new SelectList(dbService.GetAll(), "service_sub_type_id", "service_sub_type_name", service_sub_type_id);
                return View();
            }
            catch (Exception ex)
            {

                return View(ex.Message.ToString());
            }

        }
        public IActionResult BranchListReportViewer()
        {
            return View();
        }

        #region Views
        [RightPrivilegeFilter(PageIds = 800725)]
        public IActionResult LocalCustomersReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
                List<tblCustomerType> customerTypeList = new BLL.BLLCustomerType().GetAll();
                ViewBag.customerTypeList = customerTypeList;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View();
        }
        
        [RightPrivilegeFilter(PageIds = 800725)]
        public IActionResult SingleVSMultipleVisitSummaryReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }
        
        [RightPrivilegeFilter(PageIds = 800727)]
        public IActionResult AgentWiseReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800728)]
        public IActionResult ServiceSummaryReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                //int branch_id = new SessionManager(Session).branch_id;
                //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetAllBranch(), "branch_id", "branch_name", branch_id);
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800729)]
        public IActionResult GeneralSearchReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }

                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        public IActionResult AgentLogSummaryReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800730)]
        public IActionResult BreakReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    ///ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                ViewBag.break_type_id = new BLL.BLLBreakType().GetAll(); // new SelectList(new BLL.BLLBreakType().GetAll(), "break_type_id", "break_type_name");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800766)]
        public IActionResult CentreWiseSummaryReport()
        {
            try
            {
                string user_id = new SessionManager(_session).user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                ViewBag.break_type_id = new SelectList(new BLL.BLLBreakType().GetAll(), "break_type_id", "break_type_name");

                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800731)]
        public IActionResult TopNServicesReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800732)]
        public IActionResult TokenExceedingReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
                List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
                ViewBag.serviceList = serviceList;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800733)]
        public IActionResult LogOutDetailReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    ///ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        [RightPrivilegeFilter(PageIds = 800734)]
        public IActionResult LoginAttemptDetailsReport()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                string? user_id = sm.user_id;
                if (User.IsInRole("Admin"))
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(null);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(null), "branch_id", "branch_name", user_id);
                }
                else
                {
                    ViewBag.branch_id = new BLL.BLLBranch().GetBranchesByUserId(user_id);
                    //ViewBag.branch_id = new SelectList(new BLL.BLLBranch().GetBranchesByUserId(user_id), "branch_id", "branch_name", user_id);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                return View();
            }
            return View();
        }

        #endregion
        #region Exports
        public IActionResult ExportLocalCustomerReport(int branch_id, string user_id, int counter_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_LocalCustomer_report rM_Local = new RM_LocalCustomer_report();
            try
            {
                string[] headers = ["Branch Name", "Date", "Service Name", "Service Delivery Time", "Icon MSISDN", "Icon Name", "IM MSISDN", "IM Name", "Remarks", "Further follow-up needed", "Follow-up date"];
                double[] columnWidths = [100, 60, 100, 60, 60, 60, 60, 60, 60, 60, 60];
                double headerHeight = 45;
                string reportTitle = "Icon Customer Report";
                string? login_user_id = HttpContext.Session.GetString("user_id");
                List<RM_LocalCustomer> localCustomerList = null;
                if (User.IsInRole("Admin"))
                    localCustomerList = new BLL.BLLReport().LocalCustomerReport(branch_id, user_id, null, counter_id, customer_type_id, service_sub_type_id, start_date, end_date);
                else
                    localCustomerList = new BLL.BLLReport().LocalCustomerReport(branch_id, user_id, login_user_id, counter_id, customer_type_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(localCustomerList, rM_Local, report_Name, file_Type, 1, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportSinglevsMultipleVisitSummaryReport(int branch_id, string user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_SingleVSMultipleVisitedCustomer_report rM_SingleVS = new RM_SingleVSMultipleVisitedCustomer_report();
            try
            {
                string[] headers = ["Branch Name", "Service Name", "Total Served Token", "Single Visited Customers", "Multiple Visited Customers"];
                double[] columnWidths = [100, 100, 100, 100, 100];
                double headerHeight = 32;
                string reportTitle = "Single VS Multiple Visit Summary";
                string? login_user_id = sm.user_id;
                List<RM_SingleVSMultipleVisitedCustomer> singleVSMultipleVisitedList = null;
                if (User.IsInRole("Admin"))
                    singleVSMultipleVisitedList = new BLL.BLLReport().SingleVSMultipleVisitedReport(branch_id, user_id, null, counter_id, service_sub_type_id, start_date, end_date);
                else
                    singleVSMultipleVisitedList = new BLL.BLLReport().SingleVSMultipleVisitedReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(singleVSMultipleVisitedList, rM_SingleVS, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportAgentWiseSummaryReport(int branch_id, string user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_AgentWiseSummary_report rM_Agent = new RM_AgentWiseSummary_report();
            try
            {
                string[] headers = ["Branch Name", "Agent Name", "Agent id", "Handled Customers", "Average Waiting Time", "Average Service Time", "Avg. TAT"];
                double[] columnWidths = [100, 100, 60, 60, 60, 60, 60];
                double headerHeight = 35;
                string reportTitle = "Agent Wise Summery";
                string? login_user_id = sm.user_id;
                List<RM_AgentWiseSummary> agentWiseSummary = null;
                if (User.IsInRole("Admin"))
                    agentWiseSummary = new BLL.BLLReport().AgentWiseSummaryReport(branch_id, user_id, null, counter_id, service_sub_type_id, start_date, end_date);
                else
                    agentWiseSummary = new BLL.BLLReport().AgentWiseSummaryReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(agentWiseSummary, rM_Agent, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportServiceSummaryReport(int branch_id, string user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_ServiceSummary_report rM_Service = new RM_ServiceSummary_report();
            try
            {
                string[] headers = ["Branch Name", "Service Name", "Token Served", "%Total", "Standard Time", "Actual Time", "Variance"];
                double[] columnWidths = [100, 100, 80, 70, 80, 80, 60];
                double headerHeight = 22;
                string reportTitle = "Service Wise Summary";
                string? login_user_id = sm.user_id;
                List<RM_ServiceSummary> serviceSummaryList = null;
                if (User.IsInRole("Admin"))
                    serviceSummaryList = new BLL.BLLReport().ServiceSummaryReport(branch_id, user_id, null, counter_id, service_sub_type_id, start_date, end_date);
                else
                    serviceSummaryList = new BLL.BLLReport().ServiceSummaryReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(serviceSummaryList, rM_Service, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportGeneralSearchReport(int branch_id, string user_id, int counter_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_GeneralSearch_report rM_General = new RM_GeneralSearch_report();
            try
            {
                string[] headers = ["Branch Name", "Date", "Agent Name/ID", "Token No", "Mobile No", "Service Name", "Issue  Time", "Start   Time", "End  Time", "Waiting Time", "Std.  Time", "Actual Time", "Variance", "Remarks"];
                double[] columnWidths = [70, 60, 75, 50, 60, 105, 50, 50, 45, 45, 45, 45, 50, 55];
                double headerHeight = 35;
                string reportTitle = "General Search";
                string? login_user_id = sm.user_id;
                List<RM_GeneralSearch> generalSearch = null;
                if (User.IsInRole("Admin"))
                    generalSearch = new BLL.BLLReport().GeneralSearchReport(branch_id, user_id, null, counter_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);
                else
                    generalSearch = new BLL.BLLReport().GeneralSearchReport(branch_id, user_id, login_user_id, counter_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);

                return GetReportStream(generalSearch, rM_General, report_Name, file_Type, 1, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportBreakReport(int branch_id, string user_id, int counter_id, int break_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager session = new SessionManager(_session);
            RM_Break_report rM_Break = new RM_Break_report();
            try
            {
                string[] headers = ["Branch Name", "Date", "User", "Counter", "Reason For Break", "Start Time", "End Time", "Duration"];
                double[] columnWidths = [80, 50, 100, 50, 90, 60, 60, 50];
                double headerHeight = 25;
                string reportTitle = "Break Report";

                string? login_user_id = session.user_id;
                List<RM_Break> breakList = null;
                if (User.IsInRole("Admin"))
                    breakList = new BLL.BLLReport().BreakReport(branch_id, user_id, null, counter_id, break_type_id, start_date, end_date);
                else
                    breakList = new BLL.BLLReport().BreakReport(branch_id, user_id, login_user_id, counter_id, break_type_id, start_date, end_date);

                return GetReportStream(breakList, rM_Break, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportTopNServiceReport(int branch_id, string user_id, int counter_id, DateTime start_date, DateTime end_date, int topn, string report_Name = "", string file_Type = "")
        {
            SessionManager session = new SessionManager(_session);
            RM_TopNService_report rM_TopN = new RM_TopNService_report();
            try
            {
                string[] headers = ["Branch Name", "Service Name", "Total Servic"];
                double[] columnWidths = [180, 250, 120];
                double headerHeight = 25;
                string reportTitle = "Top Services Report";

                string? login_user_id = session.user_id;
                List<RM_TopNService> topNServiceList = null;
                if (User.IsInRole("Admin"))
                    topNServiceList = new BLL.BLLReport().TopNServiceReport(branch_id, user_id, null, counter_id, start_date, end_date, topn);
                else
                    topNServiceList = new BLL.BLLReport().TopNServiceReport(branch_id, user_id, login_user_id, counter_id, start_date, end_date, topn);

                return GetReportStream(topNServiceList, rM_TopN, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportTokenExceedingReport(int branch_id, string user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag, string report_Name = "", string file_Type = "")
        {
            SessionManager session = new SessionManager(_session);
            RM_TokenExceeding_report rM_Token = new RM_TokenExceeding_report();
            try
            {
                string[] headers = ["Branch Name", "User Name", "Total Served Tokens", "Total Exceeding Tokens"];
                double[] columnWidths = [100, 150, 150, 150];
                double headerHeight = 25;
                string reportTitle = "Token Exceeding Minutes";

                string? login_user_id = session.user_id;
                List<RM_TokenExceeding> tokenExceedingList = null;
                if (User.IsInRole("Admin"))
                    tokenExceedingList = new BLL.BLLReport().TokenExceedingReport(branch_id, user_id, null, counter_id, service_sub_type_id, start_date, end_date, flag);
                else
                    tokenExceedingList = new BLL.BLLReport().TokenExceedingReport(branch_id, user_id, login_user_id, counter_id, service_sub_type_id, start_date, end_date, flag);

                return GetReportStream(tokenExceedingList, rM_Token, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportLogoutDetailReport(int branch_id, string user_id, int counter_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager session = new SessionManager(_session);
            RM_LogoutDetail_Report rM_Logout = new RM_LogoutDetail_Report();
            try
            {
                string[] headers = ["Branch Name", "Agent Name", "Counter No", "Date", "Login Time", "Logout Time", "Duration", "Logout_Reason"];
                double[] columnWidths = [80, 75, 50, 65, 70, 70, 65, 90];
                double headerHeight = 27;
                string reportTitle = "Log Out Details Report";

                string? login_user_id = session.user_id;
                List<RM_LogoutDetail> logoutDetailList = null;
                if (User.IsInRole("Admin"))
                    logoutDetailList = new BLL.BLLReport().LogoutDetailReport(branch_id, user_id, null, counter_id, start_date, end_date);
                else
                    logoutDetailList = new BLL.BLLReport().LogoutDetailReport(branch_id, user_id, login_user_id, counter_id, start_date, end_date);

                return GetReportStream(logoutDetailList, rM_Logout, report_Name, file_Type, 0, headers, headerHeight, columnWidths, reportTitle);
            }
            catch (Exception ex)
            {
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportLoginAttemptDetailsReport(int branch_id, string user_id, int counter_id, int is_success, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager session = new SessionManager(_session);
            AspNetUserLoginAttempts_reprt attempts_Reprt = new AspNetUserLoginAttempts_reprt();
            try
            {
                string[] headers = ["Branch Name", "User Name", "Full Name", "Role", "Attempt Time", "Counter", "IP Address", "Machine Name", "Status"];
                double[] columnWidths = [100, 100, 80, 60, 100, 60, 80, 90, 80];
                double headerHeight = 25;
                string reportTitle = "Login Attempt Details";

                string? login_user_id = session.user_id;
                List<AspNetUserLoginAttempts> loginAttemptList = null;
                if (User.IsInRole("Admin"))
                    loginAttemptList = new BLL.BLLReport().LoginAttemptDetailsReport(branch_id, user_id, null, counter_id, is_success, start_date, end_date);
                else
                    loginAttemptList = new BLL.BLLReport().LoginAttemptDetailsReport(branch_id, user_id, login_user_id, counter_id, is_success, start_date, end_date);

                return GetReportStream(loginAttemptList, attempts_Reprt, report_Name, file_Type, 1,headers,headerHeight,columnWidths,reportTitle);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("error_message", ex.Message);
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult ExportCentreWiseSummaryReport(int branch_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            SessionManager sm = new SessionManager(_session);
            RM_CentreWiseSummary_report rM_Centre = new RM_CentreWiseSummary_report();
            try
            {
                string[] headers = ["Branch Name", "Handled Customers", "Total Served", "Average Waiting Time", "Total STD Time", "Actual Serving Time", "Variance", "Average Service Time"];
                double[] columnWidths = [100, 100, 80, 100, 100, 100, 80, 100];
                double headerHeight = 25;
                string reportTitle = "Centre Wise Summary Report";

                string login_user_id = sm.user_id;
                List<RM_CentreWiseSummary> centreWiseSummaries = null;
                if (User.IsInRole("Admin"))
                    centreWiseSummaries = new BLL.BLLReport().CentreWiseSummaryReport(branch_id, null, service_sub_type_id, start_date, end_date);
                else
                    centreWiseSummaries = new BLL.BLLReport().CentreWiseSummaryReport(branch_id, login_user_id, service_sub_type_id, start_date, end_date);
                
                return GetReportStream(centreWiseSummaries, rM_Centre, report_Name, file_Type,1,headers,headerHeight,columnWidths,reportTitle);

            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        private FileStreamResult GetReportStream<T, TModel>(List<T> list, TModel model, string report_Name, string file_Type, int orianValue, string[] headers, double headerHeight, double[] columnWidth, string reportTitle)
        {
            ExportManager exportManager = new ExportManager();
            try
            {
                if (file_Type.ToUpper() == "PDF")
                {
                    MemoryStream stream = exportManager.ExportToPdf(list, model, reportTitle, orianValue,headers,headerHeight,columnWidth);
                    stream.Position = 0;
                    return File(stream, "application/pdf", report_Name + ".pdf");
                }
                else if (file_Type.ToUpper() == "EXCEL")
                {
                    MemoryStream stream = exportManager.ExportToExcel(list, model, reportTitle, orianValue, headers, headerHeight, columnWidth);
                    stream.Position = 0;
                    return File(stream, "application/excel", report_Name + ".xlsx");
                }
                else if (file_Type.ToUpper() == "WORD")
                {
                    MemoryStream stream = exportManager.ExportToWord(list, model, reportTitle, orianValue, headers, headerHeight, columnWidth);
                    stream.Position = 0;
                    return File(stream, "application/pdf", report_Name + ".doc");
                }
                else if (file_Type.ToUpper() == "CSV")
                {
                    MemoryStream stream = exportManager.ExportToCSV(list, model, headers);
                    stream.Position = 0;
                    return File(stream, "text/csv", report_Name + ".csv");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        #endregion
    }
}
