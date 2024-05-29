using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Diagnostics;

namespace SQMS.Controllers
{
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        private BLL.BLLBranch dbBranch = new BLL.BLLBranch();
        private BLL.BLLCounters dbCounter = new BLL.BLLCounters();
        private BLL.BLLBranchUsers dbUser = new BLL.BLLBranchUsers();
        private BLL.BLLAspNetUser db_User = new BLL.BLLAspNetUser();
        private readonly IHttpContextAccessor _session;
        private readonly notifyDisplay _notifyDisplay;
        private readonly IHubContext<notifyDisplay> _hubContext;

        public HomeController(IHttpContextAccessor Session, notifyDisplay notifyDisplay, IHubContext<notifyDisplay> hubContext)
        {
            _session = Session;  
            _notifyDisplay = notifyDisplay;
            _hubContext = hubContext;
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public IActionResult Index()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                int branch_id = 0;
                if (!User.IsInRole("Admin"))
                {
                    branch_id = sm.branch_id;
                }
                List<tblBranch> branchList = new List<tblBranch>();
                branchList = dbBranch.GetAllBranch();
                ViewBag.branchList = branchList;
                ViewBag.userBranchId = branch_id;
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View();
        }

        [AuthorizationFilter(Roles = "Admin, Branch Admin, Token Generator")]
        public IActionResult Home()
        {
            return View();
        }

        [AuthorizationFilter]
        public IActionResult GetUserAndCounterByBranchId(int branchId, bool isOnlyServiceHolder)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                var userList = dbUser.GetAll().Where(a => (!isOnlyServiceHolder && a.branch_id == branchId) || (a.branch_id == branchId && a.Name == "Service Holder")).Select(x => new
                {
                    x.user_id,
                    user_name = (isOnlyServiceHolder ? x.Hometown : x.UserName)
                }).ToList();

                var counterList = dbCounter.GetAllCounter().Where(a => a.branch_id == branchId).Select(x => new
                {
                    x.counter_id,
                    x.counter_no
                }).ToList();
                return Ok(new { success = true, userList = userList, counterList = counterList });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult BranchLogin()
        {
            return View();
        }


        //[AuthorizationFilter(Roles = "Admin")]
        //[RightPrivilegeFilter(PageIds = 800765)]
        public IActionResult AdminDashboard()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                int branch_id = new SessionManager(_session).branch_id;
                ViewBag.branch_id = dbBranch.GetAllBranch();
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View();
        }

        [AuthorizationFilter(Roles = "Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800704)]
        public IActionResult BranchSelection()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                List<tblBranch> branchList = dbBranch.GetBranchesByUserId(sm.user_id);
                
                if (branchList.Count ==1)
                {
                    tblBranch branch = branchList.FirstOrDefault();
                    sm.branch_id = branch.branch_id;
                    sm.branch_name = branch.branch_name;
                    new BLL.BLLAspNetUser().UpdateLoginInfo(sm.LoginProvider, sm.branch_id);                    
                    return RedirectToAction("BranchAdminDashboard", "Home");
                }
                ViewBag.branchList = branchList;
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
            return View();
        }

        [AuthorizationFilter(Roles = "Branch Admin")]
        [HttpPost]
        public IActionResult BranchSelection(BranchLoginModel model)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (model.branch_id != 0)
                {
                    sm.branch_id = model.branch_id;
                    sm.branch_name = dbBranch.GetById(model.branch_id).branch_name;
                    new BLL.BLLAspNetUser().UpdateBranchAdminLoginInfo(sm.LoginProvider, sm.branch_id);
                    return RedirectToAction("BranchAdminDashboard", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [AuthorizationFilter(Roles = "Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800705)]
        public IActionResult BranchAdminDashboard()
        {
            return View();
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        [RightPrivilegeFilter(PageIds = 800700)]
        public IActionResult CounterSelection()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                int branch_id = 0;
                string branch_name = "";
                if (!User.IsInRole("Admin"))
                {

                    branch_id = sm.branch_id;
                    branch_name = sm.branch_name;
                }

                List<tblCounter> counterList = dbCounter.GetFree(branch_id, sm.user_id);
                ViewBag.counterList = counterList;
                ViewBag.branch_id = branch_id;
                ViewBag.branch_name = branch_name;
                ViewBag.VMBranchCounterStatus = new BLL.BLLBranch().GetCounterCurrentStatus(sm.branch_id, 0);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View();
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        [HttpPost]
        public async Task<ActionResult> CounterSelection(BranchLoginModel model)
        {
            SessionManager sm = new SessionManager(_session);
            notifyDisplay notifyDisplay2 = new notifyDisplay(_hubContext);
            try
            {
                if (ModelState.IsValid)
                {
                    sm.counter_idv2 = model.counter_id.ToString();
                    sm.counter_no = dbCounter.GetAllCounter().Where(w => w.counter_id == model.counter_id).FirstOrDefault().counter_no;
                    new BLL.BLLAspNetUser().UpdateLoginInfo(sm.LoginProvider, model.counter_id);
                                        
                    await notifyDisplay2.CounterStatusChanged(sm.branch_id);
                    
                    return RedirectToAction("Create", "ServiceDetails");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
    }
}
