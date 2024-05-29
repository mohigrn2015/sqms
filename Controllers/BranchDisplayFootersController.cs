using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class BranchDisplayFootersController : Controller
    {
        private readonly BLLBranchDisplayFooter dbManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _hubContext;
        public BranchDisplayFootersController(BLLBranchDisplayFooter _dbManager, notifyDisplay _notifyDisplay, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor session, IHubContext<notifyDisplay> hubContext)
        {
            this.dbManager = _dbManager;
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;
            _session = session;
        }
        // GET: DisplayFooters
        [RightPrivilegeFilter(PageIds = 800754)]
        public IActionResult Index()
        {
            try
            {
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // GET: Branches/Create
        [RightPrivilegeFilter(PageIds = 800755)]
        public IActionResult Create()
        {
            try
            {
                List<tblBranch> branchList = new BLLBranch().GetAllBranch();
                List<VMBranchDisplayFooter> displayFootersList = dbManager.GetAll();

                var qry = branchList.GroupJoin(
                          displayFootersList,
                          b => b.branch_id,
                          d => d.branch_id,
                          (x, y) => new { branchList = x, displayFootersList = y })
                          .Where(w => w.displayFootersList.Count() == 0)
                          .Select(
                          x => x.branchList).ToList();

                ViewBag.branch_id = qry;
                ViewBag.display_footer_id = new BLLDisplayFooter(_webHostEnvironment).GetAll();
                return View();
            } 
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("branch_display_footer_id, branch_id, display_footer_id")] VMBranchDisplayFooter displayFooter)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(displayFooter);

                    await notifyDisplay.SendMessages(displayFooter.branch_id, "", "", false, false, false, true);
                    return RedirectToAction("Index");
                }

                List<tblBranch> branchList = new BLLBranch().GetAllBranch();
                List<VMBranchDisplayFooter> displayFootersList = dbManager.GetAll();

                var qry = branchList.GroupJoin(
                          displayFootersList,
                          b => b.branch_id,
                          d => d.branch_id,
                          (x, y) => new { branchList = x, displayFootersList = y })
                          .Where(w => w.displayFootersList.Count() == 0)
                          .Select(
                          x => x.branchList).ToList();

                ViewBag.branch_id = qry;
                //ViewBag.branch_id = new SelectList(qry, "branch_id", "branch_name");
                ViewBag.display_footer_id = new BLLDisplayFooter(_webHostEnvironment).GetAll();
                //ViewBag.display_footer_id = new SelectList(new BLLDisplayFooter(_webHostEnvironment).GetAll(), "display_footer_id", "content_en");
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800756)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                VMBranchDisplayFooter displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                ViewBag.display_footer_id = new SelectList(new BLLDisplayFooter(_webHostEnvironment).GetAll(), "display_footer_id", "content_en", displayFooter.display_footer_id);
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("branch_display_footer_id, branch_id, display_footer_id")] VMBranchDisplayFooter displayFooter)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(displayFooter);
                    await notifyDisplay.SendMessages(displayFooter.branch_id, "", "", false, false, false, true);
                    return RedirectToAction("Index");
                }
                ViewBag.display_footer_id = new SelectList(new BLLDisplayFooter(_webHostEnvironment).GetAll(), "display_footer_id", "content_en", displayFooter.display_footer_id);
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // GET: Branches/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                VMBranchDisplayFooter displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                return View(displayFooter);
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                VMBranchDisplayFooter displayFooter = dbManager.GetById(id);
                if (displayFooter == null)
                {
                    return NotFound();
                }
                dbManager.Remove(displayFooter.display_footer_id);
                await notifyDisplay.SendMessages(displayFooter.branch_id, "", "", false, false, false, true);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

    }
}
