using Microsoft.AspNetCore.Mvc;
using SQMS.BLL;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    //[AuthorizationFilter(Roles = "Admin")]
    public class DisplayFootersController : Controller
    {
        private readonly BLLDisplayFooter dbManager;
        private readonly notifyDisplay notifyDisplay;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor _session;

        public DisplayFootersController(BLLDisplayFooter dbManager, notifyDisplay notifyDisplay, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor session)
        {
            this.dbManager = dbManager;
            this.notifyDisplay = notifyDisplay;
            this.webHostEnvironment = _webHostEnvironment;
            _session = session; 
        }
        // GET: DisplayFooters
        //[RightPrivilegeFilter(PageIds = 800750)]
        public IActionResult Index()
        {
            try
            {
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // GET: Branches/Create
        [RightPrivilegeFilter(PageIds = 800751)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("display_footer_id,content_en,content_bn,is_global")] tblDisplayFooter displayFooter)
        {
            try
            {
                displayFooter.is_global = 0;
                
                if (ModelState.IsValid)
                {
                    dbManager.Create(displayFooter);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View(displayFooter);
        }

        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800752)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblDisplayFooter displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            } 
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("display_footer_id,content_en,content_bn,is_global")] tblDisplayFooter displayFooter)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    dbManager.Edit(displayFooter);

                    if (displayFooter.is_global > 0)
                        await notifyDisplay.SendMessages(0, "", "", false, false, false, true);

                    var branchDisplayFooter = new BLLBranchDisplayFooter(webHostEnvironment).GetAll().Where(w => w.display_footer_id == displayFooter.display_footer_id).FirstOrDefault();
                    if (branchDisplayFooter != null)
                        await notifyDisplay.SendMessages(branchDisplayFooter.branch_id, "", "", false, false, false, true);

                    return RedirectToAction("Index");
                }
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblDisplayFooter displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return NotFound();
                }

                var branchDisplayFooter = new BLLBranchDisplayFooter(webHostEnvironment).GetAll().Where(w => w.display_footer_id == displayFooter.display_footer_id).FirstOrDefault();
                if (branchDisplayFooter != null)
                    await notifyDisplay.SendMessages(branchDisplayFooter.branch_id, "", "", false, false, false, true);

                return View(displayFooter);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                tblDisplayFooter displayFooter = dbManager.GetById(id);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                dbManager.Remove(displayFooter.display_footer_id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800753)]
        public async Task<IActionResult> SetNational(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblDisplayFooter displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                displayFooter.is_global = 1;
                dbManager.Edit(displayFooter);
                await notifyDisplay.SendMessages(0, "", "", false, false, false, true);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }
    }
}
