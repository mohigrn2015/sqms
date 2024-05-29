using Microsoft.AspNetCore.Mvc;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class LogoutTypesController : Controller
    {
        private readonly IHttpContextAccessor _session;
        private readonly BLLLogoutType dbManager = new BLLLogoutType();
        public LogoutTypesController(IHttpContextAccessor httpContext)
        {
             _session = httpContext;
        }
        // GET: LogoutTypes
        [RightPrivilegeFilter(PageIds = 800737)]
        
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

        // GET: LogoutTypes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblLogoutType tblLogoutType = await db.tblLogoutTypes.FindAsync(id);
        //    if (tblLogoutType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblLogoutType);
        //}

        // GET: LogoutTypes/Create
        [RightPrivilegeFilter(PageIds = 800738)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogoutTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create([Bind("logout_type_id,logout_type_name,bool_has_free_text,bool_is_active")] tblLogoutType tblLogoutType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(tblLogoutType);
                    return RedirectToAction("Index");
                }
                return View(tblLogoutType);

            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // GET: LogoutTypes/Edit/5
        [RightPrivilegeFilter(PageIds = 800739)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblLogoutType tblLogoutType = dbManager.GetById(id.Value);
                if (tblLogoutType == null)
                {
                    return NotFound();
                }
                return View(tblLogoutType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: LogoutTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("logout_type_id,logout_type_name,bool_has_free_text,bool_is_active")] tblLogoutType tblLogoutType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblLogoutType);
                    return RedirectToAction("Index");
                }
                return View(tblLogoutType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: LogoutTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblLogoutType tblLogoutType = dbManager.GetById(id.Value);
                if (tblLogoutType == null)
                {
                    return NotFound();
                }
                return View(tblLogoutType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: LogoutTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblLogoutType tblLogoutType = dbManager.GetById(id);
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
