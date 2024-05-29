using Microsoft.AspNetCore.Mvc;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    ///[AuthorizationFilter(Roles = "Admin")]
    public class ServiceTypesController : Controller
    {
        private readonly BLLServiceType dbManager = new BLLServiceType();
        private readonly IHttpContextAccessor _session;
        public ServiceTypesController(IHttpContextAccessor accessor)
        {
            _session = accessor;
        }
        // GET: ServiceTypes
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

        //[AuthorizationFilter]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(new { Success = true, serviceTypes = dbManager.GetAll() });
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        //[RightPrivilegeFilter(PageIds = 800743)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tblServiceType tblServiceType)
        {
            try
            {
                if (!String.IsNullOrEmpty(tblServiceType.service_type_name))
                {
                    //db.tblServiceTypes.Add(tblServiceType);
                    dbManager.Create(tblServiceType);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            return View(tblServiceType);
        }
        // GET: ServiceTypes/Edit/5
        //[RightPrivilegeFilter(PageIds = 800744)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblServiceType tblServiceType = dbManager.GetById(id.Value);
                if (tblServiceType == null)
                {
                    return NotFound();
                }
                return View(tblServiceType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("service_type_id,service_type_name")] tblServiceType tblServiceType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblServiceType);
                    return RedirectToAction("Index");
                }
                return View(tblServiceType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: ServiceTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblServiceType tblServiceType = dbManager.GetById(id.Value);
                if (tblServiceType == null)
                {
                    return NotFound();
                }
                return View(tblServiceType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: ServiceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblServiceType tblServiceType = dbManager.GetById(id);
                dbManager.Remove(id);
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
