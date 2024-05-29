using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class BranchPlayListController : Controller
    {
        private readonly BLLBranchPlayList dbManager = new BLLBranchPlayList();        
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _context;
        public BranchPlayListController(notifyDisplay _notifyDisplay, IHttpContextAccessor accessor, IHubContext<notifyDisplay> context)
        {
            _context = context;
            _session = accessor;

        }
        // GET: DisplayFooters
        [RightPrivilegeFilter(PageIds = 800762)]
        public IActionResult Index()
        {
            try
            {
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // GET: Branches/Create
        [RightPrivilegeFilter(PageIds = 800763)]
        public IActionResult Create()
        {
            try
            {
                List<tblBranch> branchList = new BLLBranch().GetAllBranch();
                List<VMBranchPlayList> branchPlayList = dbManager.GetAll();

                var qry = branchList.GroupJoin(
                          branchPlayList,
                          b => b.branch_id,
                          d => d.branch_id,
                          (x, y) => new { branchList = x, branchPlayList = y })
                          .Where(w => w.branchPlayList.Count() == 0)
                          .Select(
                          x => x.branchList).ToList();

                ViewBag.branch_id = qry;
                //ViewBag.branch_id = new SelectList(qry, "branch_id", "branch_name");
                ViewBag.playlist_id = new BLLPlayList().GetAll();
                //ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name");
                return View();
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("branch_playlist_id, branch_id, playlist_id")] VMBranchPlayList playList)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(playList);

                    await notifyDisplay.SendMessages(playList.branch_id, "", "", false, false, false, true);
                    return RedirectToAction("Index");
                }

                List<tblBranch> branchList = new BLLBranch().GetAllBranch();
                List<VMBranchPlayList> branchPlayList = dbManager.GetAll();



                var qry = branchList.GroupJoin(
                          branchPlayList,
                          b => b.branch_id,
                          d => d.branch_id,
                          (x, y) => new { branchList = x, branchPlayList = y })
                          .Where(w => w.branchPlayList.Count() == 0)
                          .Select(
                          x => x.branchList).ToList();

                ViewBag.branch_id = qry;
                ViewBag.playlist_id = new BLLPlayList().GetAll();
                return View(branchPlayList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800764)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                VMBranchPlayList branchPlayList = dbManager.GetById(id.Value);
                if (branchPlayList == null)
                {
                    return NotFound();
                }
                ViewBag.playlist_id = new BLLPlayList().GetAll();
                //ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", branchPlayList.playlist_id);
                return View(branchPlayList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("branch_playlist_id, branch_id, playlist_id")] VMBranchPlayList branchPlayList)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(branchPlayList);
                    await notifyDisplay.SendMessages(branchPlayList.branch_id, "", "", false, false, false, true);
                    return RedirectToAction("Index");
                }
                ViewBag.playlist_id =new BLLPlayList().GetAll();
                //ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", branchPlayList.playlist_id);
                return View(branchPlayList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
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
                VMBranchPlayList branchPlayList = dbManager.GetById(id.Value);
                if (branchPlayList == null)
                {
                    return NotFound();
                }
                return View(branchPlayList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                VMBranchPlayList branchPlayList = dbManager.GetById(id);
                if (branchPlayList == null)
                {
                    return NotFound();
                }
                dbManager.Remove(id);
                await notifyDisplay.SendMessages(branchPlayList.branch_id, "", "", false, false, false, true);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }
    }
}
