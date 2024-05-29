using Microsoft.AspNetCore.Mvc;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class BreakTypesController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private readonly BLLBreakType dbManager = new BLLBreakType();
        private readonly IHttpContextAccessor _session;

        public BreakTypesController(IHttpContextAccessor accessor)
        {
            _session = accessor;
        }

        [RightPrivilegeFilter(PageIds = 800748)]
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

        // GET: BreakTypes/Create
        [RightPrivilegeFilter(PageIds = 800748)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BreakTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("break_type_id,break_type_short_name,break_type_name,start_time,end_time,duration")] tblBreakType tblBreakType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(tblBreakType);
                    return RedirectToAction("Index");
                }

                return View(tblBreakType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
        }

        // GET: BreakTypes/Edit/5
        [RightPrivilegeFilter(PageIds = 800749)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var tblBreakType = dbManager.GetById(id.Value);
                if (tblBreakType == null)
                {
                    return NotFound();
                }
                return View(tblBreakType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: BreakTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("break_type_id,break_type_short_name,break_type_name,start_time,end_time,duration")] tblBreakType tblBreakType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblBreakType);
                    return RedirectToAction("Index");
                }
                return View(tblBreakType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: BreakTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblBreakType tblBreakType = dbManager.GetById(id.Value);
                if (tblBreakType == null)
                {
                    return NotFound();
                }
                return View(tblBreakType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: BreakTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblBreakType tblBreakType = dbManager.GetById(id);
                dbManager.Remove(id);
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
