using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin,Branch Admin,Service Holder")]
    public class DailyBreaksController : Controller
    {
        private readonly BLLDailyBreak dbManager = new BLLDailyBreak();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly BLLBreakType dbBreak = new BLLBreakType();
        private readonly BLLAspNetUser dbUser = new BLLAspNetUser();
        private readonly BLLAspNetRole dbRole = new BLLAspNetRole();
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _context; 
        public DailyBreaksController(notifyDisplay _notifyDisplay,IHttpContextAccessor session, IHubContext<notifyDisplay> context)
        {
            _context = context;
            _session = session;
        }

        //GET: DailyBreaks
        [RightPrivilegeFilter(PageIds = 800703)]
        public IActionResult Index()
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
                return View(dbManager.GetAll(branch_id, user_id));
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [HttpPost]
        public IActionResult GetCountByUserId()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                int is_break = dbManager.GetCountByUserId(sm.user_id);

                return Ok(new { success = true, is_break = is_break });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Message = ex.Message });

            }
        }

        // GET: DailyBreaks/Create
        public IActionResult Create()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                ViewBag.user_id = dbUser.GetAllUser().Where(x => x.Id == sm.user_id);
                //ViewBag.user_id = new SelectList(dbUser.GetAllUser(), "Id", "Hometown", sm.user_id);

                ViewBag.break_type_id = dbBreak.GetAll();
                return PartialView();
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: DailyBreaks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(int break_type_id, string remarks)
        {
            try
            {
                tblDailyBreak dailyBreak = new tblDailyBreak();
                SessionManager sm = new SessionManager(_session);
                dailyBreak.break_type_id = break_type_id;
                dailyBreak.remarks = remarks;
                dailyBreak.counter_id = sm.counter_id;
                dailyBreak.user_id = sm.user_id;
                dailyBreak.start_time = DateTime.Now;
                dbManager.Create(dailyBreak);
                return Ok(new { Success = true, Message = "Successfully added" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = ex.Message });
            }
        }

        // GET: DailyBreaks/Edit/5
        public IActionResult Edit(int? id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblDailyBreak tblDailyBreak = dbManager.GetById(id.Value);
                if (tblDailyBreak == null)
                {
                    return NotFound();
                }
                ViewBag.user_id = new SelectList(dbUser.GetAllUser(), "Id", "Hometown", tblDailyBreak.user_id);
                ViewBag.break_type_id = new SelectList(dbBreak.GetAll(), "break_type_id", "break_type_short_name", tblDailyBreak.break_type_id);
                return View(tblDailyBreak);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update()
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                SessionManager sm = new SessionManager(_session);
                string user_id = sm.user_id;
                int counter_id = sm.counter_id;
                string counter_no = sm.counter_no;
                if (ModelState.IsValid)
                {
                    dbManager.Update(user_id);
                    await notifyDisplay.SendMessages(sm.branch_id, sm.counter_no, "", false, true, false, false);
                    return Ok(new { Success = true, Message = "Successfully added" });
                }
                return Ok(new { Success = false, Message = "Failed for parameter missing" });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = ex.Message });

            }
        }
        // GET: DailyBreaks/Delete/5
        public IActionResult Delete(int? id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblDailyBreak tblDailyBreak = dbManager.GetById(id.Value);
                if (tblDailyBreak == null)
                {
                    return NotFound();
                }
                return View(tblDailyBreak);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: DailyBreaks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                tblDailyBreak tblDailyBreak = dbManager.GetById(id);
                dbManager.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
    } 
}
