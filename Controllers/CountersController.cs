using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.ResponseModel;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    public class CountersController : Controller
    {
        private readonly BLLCounters dbManager = new BLLCounters();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly DisplayManager dm = new DisplayManager();
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _context;        
        public CountersController(notifyDisplay _notifyDisplay, IHttpContextAccessor session, IHubContext<notifyDisplay> context)
        {
            _context = context;
            _session = session;
        }


        // GET: Counters
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800712)]
        public IActionResult Index()
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                ViewBag.userBranchId = sessionManager.branch_id;
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            //var tblCounters = db.tblCounters.Include(t => t.tblBranch);
            //return View(await tblCounters.ToListAsync());
        }

        // GET: Counters/Create
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800713)]
        [RightPrivilegeFilter(PageIds = 800713)]
        [RightPrivilegeFilter(PageIds = 800713)]
        public IActionResult Create()
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                int branch_id = sessionManager.branch_id;
                ViewBag.branch_id = dbBranch.GetAllBranch();
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", branch_id);
                return View();
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        //[AuthorizationFilter(Roles = "Admin, Branch Admin, Display User")]
        [AllowAnonymous]
        public IActionResult CounterList()
        {
            SessionManager sessionManager = new SessionManager(_session);
            ViewBag.branch_id = sessionManager.branch_id;
            ViewBag.dispalyFooterAdd = ApplicationSetting.dispalyFooterAdd;
            ViewBag.dispalyWelcome = ApplicationSetting.dispalyWelcome;
            ViewBag.dispalyVideo = ApplicationSetting.dispalyVideo;
            ViewBag.speakLanguage = ApplicationSetting.speakLanguage;
            ViewBag.speakGender = ApplicationSetting.speakGender;
            ViewBag.speakRate = ApplicationSetting.speakRate;
            return View();
        }

        //[AuthorizationFilter(Roles = "Admin, Branch Admin, Display User")]
        [AllowAnonymous]
        public IActionResult GetDisplayInfo(int branch_id)
        {
            SessionManager sessionManager = new SessionManager(_session);
            CounterResponseModel counter = new CounterResponseModel();
            CommonResponseModel common = new CommonResponseModel();
            try
            {
                var tokenInProgress = dm.GetInProgressTokenList(branch_id);

                string nextToken = dm.GetNextTokens(branch_id);

                counter = new CounterResponseModel()
                {
                    success = true,
                    tokenInProgress = tokenInProgress,
                    nextTokens = nextToken,
                    dispalyVideoUrl = ApplicationSetting.dispalyVideo
                };

                return Ok(counter);
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
        }

        // POST: Counters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<IActionResult> Create([Bind("counter_id,branch_id,counter_no,location")] tblCounter tblCounter)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    tblCounter.branch_id = sessionManager.branch_id;
                }
                else
                {
                    tblCounter.branch_id = tblCounter.branch_id;
                }

                if (ModelState.IsValid)
                {
                    dbManager.Create(tblCounter);

                    await notifyDisplay.SendMessages(tblCounter.branch_id, tblCounter.counter_no, "", false, true, false, false);

                    return RedirectToAction("Index");
                }

                ViewBag.branch_id = dbBranch.GetAllBranch();
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", tblCounter.branch_id);
                return View(tblCounter);
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
        }

        // GET: Counters/Edit/5
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800714)]
        public ActionResult Edit(int? id)
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCounter tblCounter = dbManager.GetById(id.Value);
                if (tblCounter == null)
                {
                    return NotFound();
                }
                //ViewBag.branch_id = dbBranch.GetAllBranch();
                ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", tblCounter.branch_id);
                return View(tblCounter);
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Counters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<ActionResult> Edit([Bind("counter_id,branch_id,counter_no,location,is_active")] tblCounter tblCounter)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblCounter);
                    await notifyDisplay.SendMessages(tblCounter.branch_id, tblCounter.counter_no, "", false, true, false, false);

                    return RedirectToAction("Index");
                }

                ViewBag.branch_id = dbBranch.GetAllBranch();
                return View(tblCounter);
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<ActionResult> Activate(int? id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCounter tblCounter = dbManager.GetById(id.Value);
                if (tblCounter == null)
                {
                    return NotFound();
                }
                tblCounter.is_active = 1;
                dbManager.Edit(tblCounter);
                await notifyDisplay.SendMessages(tblCounter.branch_id, tblCounter.counter_no, "", false, true, false, false);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<ActionResult> Deactivate(int? id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCounter tblCounter = dbManager.GetById(id.Value);
                if (tblCounter == null)
                {
                    return NotFound();
                }
                tblCounter.is_active = 0;
                dbManager.Edit(tblCounter);

                await notifyDisplay.SendMessages(tblCounter.branch_id, tblCounter.counter_no, "", false, true, false, false);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }          
        }

        // GET: Counters/Delete/5
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public ActionResult Delete(int? id)
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCounter tblCounter = dbManager.GetById(id.Value);
                if (tblCounter == null)
                {
                    return NotFound();
                }
                return View(tblCounter);
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Counters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                tblCounter tblCounter = dbManager.GetById(id);
                dbManager.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sessionManager.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Display(int id)
        {
            ViewBag.branch_id = id;
            ViewBag.dispalyFooterAdd = ApplicationSetting.dispalyFooterAdd;
            ViewBag.dispalyWelcome = ApplicationSetting.dispalyWelcome;
            ViewBag.dispalyVideo = ApplicationSetting.dispalyVideo;
            return View();
        }


        // GET: Counters
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult GetCounterByBrunchId(int id)
        {
            SessionManager sessionManager = new SessionManager(_session);
            try
            {
                var tblCounters = dbManager.GetCounterByBrunchId(id);

                return Ok(new { success = "true", counters = tblCounters });
            }
            catch (Exception ex)
            {
                return Ok(new { success = "false", message = ex.Message });
            }
        }
    }
}
