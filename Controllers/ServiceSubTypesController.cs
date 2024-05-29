using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.RequestModel;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    //[AuthorizationFilter(Roles = "Admin")]
    public class ServiceSubTypesController : Controller
    {
        private readonly BLLServiceSubType dbManager = new BLLServiceSubType();
        private readonly BLLServiceType dbService = new BLLServiceType();
        private readonly BLLGlobalSettings globalSettings = new BLLGlobalSettings();
        private readonly IHttpContextAccessor _session;
        public ServiceSubTypesController(IHttpContextAccessor accessor)
        {
            _session = accessor;
        }
        // GET: ServiceSubTypes
        //[RightPrivilegeFilter(PageIds = 800745)]
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

        [AllowAnonymous]
        public IActionResult GetByTypeId(ServiceSubTypeReqModel model)
        {
            try
            {
                var subTypes = dbManager.GetByTypeId(model.service_type_id, 1);
                int globalSet = new BLLGlobalSettings().Get().tat_visibility_time;
                return Ok(new { Success = true, serviceSubTypes = subTypes, globalSet = globalSet });
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // GET: ServiceSubTypes/Create
        //[RightPrivilegeFilter(PageIds = 800746)]
        public IActionResult Create()
        {
            try
            {
                ViewBag.service_type_id = dbService.GetAll();
                return View();
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // POST: ServiceSubTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("service_sub_type_id,service_type_id,service_sub_type_name,max_duration,tat_warning_time")] tblServiceSubType tblServiceSubType)
        {
            try
            {
                if (!String.IsNullOrEmpty(tblServiceSubType.service_sub_type_name))
                {
                    dbManager.Create(tblServiceSubType);
                    return RedirectToAction("Index");
                }
                //ViewBag.service_type_id = new SelectList(dbService.GetAll(), "service_type_id", "service_type_name", tblServiceSubType.service_type_id);
                return View(tblServiceSubType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: ServiceSubTypes/Edit/5
        //[RightPrivilegeFilter(PageIds = 800747)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblServiceSubType tblServiceSubType = dbManager.GetById(id.Value);
                if (tblServiceSubType == null)
                {
                    return NotFound();
                }
                ViewBag.service_type_id = dbService.GetAll();
                return View(tblServiceSubType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: ServiceSubTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("service_sub_type_id,service_type_id,service_sub_type_name,max_duration,tat_warning_time")] tblServiceSubType tblServiceSubType)
        {
            try
            {               
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblServiceSubType);
                    return RedirectToAction("Index");
                }
                ViewBag.service_type_id = dbService.GetAll();
                return View(tblServiceSubType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        [HttpPost]
        public IActionResult SetStatus(int service_sub_type_id, int is_activate)
        {            
            try
            {
                dbManager.SetStatus(service_sub_type_id, is_activate);
                return Ok(new { success = "true", message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = "false", message = ex.Message });
            }

        }

        // GET: ServiceSubTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblServiceSubType tblServiceSubType = dbManager.GetById(id.Value);
                if (tblServiceSubType == null)
                {
                    return NotFound();
                }
                return View(tblServiceSubType);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: ServiceSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblServiceSubType tblServiceSubType = dbManager.GetById(id);
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

        [HttpPost]
        public IActionResult EditServiceTatTimeBulk(string service_id, int time)
        {
            try
            {
                dbManager.UpdateTatBulk(service_id, time);
                return Ok(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
    }
}
