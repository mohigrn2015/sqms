using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SQMS.BLL;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class PlayListsController : Controller
    {
        private BLLPlayList dbManager = new BLLPlayList();
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _context;
        public PlayListsController(notifyDisplay notifyDisplay, IHttpContextAccessor session, IHubContext<notifyDisplay> context)
        {
            _context = context; 
            _session = session;
        }
        // GET: DisplayFooters
        [RightPrivilegeFilter(PageIds = 800759)]
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
        [RightPrivilegeFilter(PageIds = 800760)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tblPlayList playList)
        {
            try
            {
                playList.is_global = 0;
               
                if (!String.IsNullOrEmpty(playList.playlist_name))
                {
                    dbManager.Create(playList);

                    return RedirectToAction("Index");
                }
                return View(playList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Delete/5
        public IActionResult Detail(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblPlayList playList = dbManager.GetById(id.Value);
                if (playList == null)
                {
                    return NotFound();
                }
                return View(playList);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }


        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800761)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblPlayList playList = dbManager.GetById(id.Value);
                if (playList == null)
                {
                    return NotFound();
                }
                return View(playList);
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
        public IActionResult Edit([Bind("playlist_id,playlist_name,is_global")] tblPlayList playList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(playList);
                    return RedirectToAction("Index");
                }
                return View(playList);
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
                tblPlayList playList = dbManager.GetById(id.Value);
                if (playList == null)
                {
                    return NotFound();
                }
                return View(playList);
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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                tblPlayList playList = dbManager.GetById(id);
                if (playList == null)
                {
                    return NotFound();
                }
                dbManager.Remove(playList.playlist_id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> SetNational(int? id)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_context);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblPlayList playList = dbManager.GetById(id.Value);
                if (playList == null)
                {
                    return NotFound();
                }
                playList.is_global = 1;
                dbManager.Edit(playList);
                await notifyDisplay.SendMessages(0, "", "", false, false, true, false);

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
