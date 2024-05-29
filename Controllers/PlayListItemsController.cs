using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SQMS.BLL;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    //[AuthorizationFilter(Roles = "Admin")]
    public class PlayListItemsController : Controller
    {
        private BLLPlayListItem dbManager = new BLLPlayListItem();
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _hubContext;

        public PlayListItemsController(notifyDisplay notifyDisplay, IHttpContextAccessor session, IHubContext<notifyDisplay> hubContext)
        {
            _hubContext = hubContext;
            _session = session;
        }
        // GET: DisplayFooters
        public IActionResult Index(int playlist_id)
        {
            try
            {
                ViewBag.playlist_id = playlist_id;
                tblPlayList playList = new BLLPlayList().GetById(playlist_id);
                if (playList != null)
                {
                    ViewBag.playlist_name = playList.playlist_name;
                }
                else ViewBag.playlist_name = "Not Found";

                return View(dbManager.GetAll(playlist_id));
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // GET: Branches/Create
        public IActionResult Create(int playlist_id = 0)
        {
            try
            {
                if (playlist_id == 0)
                {
                    return BadRequest();
                }
                tblPlayList playList = new BLLPlayList().GetById(playlist_id);
                if (playList == null)
                {
                    return NotFound();
                }
                tblPlayListItem playListItem = new tblPlayListItem()
                {
                    playlist_id = playList.playlist_id,
                    playlist_name = playList.playlist_name,
                    item_url = ApplicationSetting.galleryDefaultPath
                };
                return View(playListItem);
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(tblPlayListItem playListItem)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (playListItem.playlist_id != 0) 
                {
                    playListItem.item_url = ApplicationSetting.galleryDBPath;
                    playListItem.file_type = playListItem.getFileType();

                    dbManager.Create(playListItem);
                    await notifyDisplay.SendMessages(0, "", "", false, false, true, false);
                    return RedirectToAction("Index", new { playListItem.playlist_id });
                }
                return View(playListItem);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // GET: Branches/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblPlayListItem playListItem = dbManager.GetById(id.Value);
                if (playListItem == null)
                {
                    return NotFound();
                }
                return View(playListItem);
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
        public async Task<IActionResult> Edit(tblPlayListItem playListItem)
        {
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (playListItem.playlist_id != 0)
                {
                    playListItem.item_url = ApplicationSetting.galleryDBPath;
                    playListItem.file_type = playListItem.getFileType();
                    dbManager.Edit(playListItem);

                    await notifyDisplay.SendMessages(0, "", "", false, false, true, false);
                    return RedirectToAction("Index", new { playListItem.playlist_id });
                }
                return View(playListItem);
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
                tblPlayListItem playListItem = dbManager.GetById(id.Value);
                if (playListItem == null)
                {
                    return NotFound();
                }
                return View(playListItem);
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
            notifyDisplay notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                tblPlayListItem playListItem = dbManager.GetById(id);
                if (playListItem == null)
                {
                    return NotFound();
                }
                dbManager.Remove(id);
                await notifyDisplay.SendMessages(0, "", "", false, false, true, false);

                return RedirectToAction("Index", new { playListItem.playlist_id });
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
