using Microsoft.AspNetCore.Mvc;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin")]
    public class CustomerTypesController : Controller
    { 
        private readonly BLLCustomerType dbCustomerType;
        private readonly IHttpContextAccessor _session;

        public CustomerTypesController(BLLCustomerType _dbCustomerType, IHttpContextAccessor session)
        {
            this.dbCustomerType = _dbCustomerType;
            _session = session;
        }

        public IActionResult Index()
        {
            try
            {
                return View(dbCustomerType.GetAll());
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var tblCustomerType = dbCustomerType.GetById(id.Value);
                if (tblCustomerType == null)
                {
                    return NotFound();
                }
                return View(tblCustomerType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }


        // GET: CustomerTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("customer_type_id,customer_type_name,priority,token_prefix")] tblCustomerType tblCustomerType)
        {
            try
            {
                tblCustomerType.is_default = 0;
                tblCustomerType.token_prefix = tblCustomerType.token_prefix.ToUpper();

                if (ModelState.IsValid)
                {
                    dbCustomerType.Create(tblCustomerType);
                    return RedirectToAction("Index");
                }

                return View(tblCustomerType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: CustomerTypes/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCustomerType tblCustomerType = dbCustomerType.GetById(id.Value);
                if (tblCustomerType == null)
                {
                    return NotFound();
                }
                return View(tblCustomerType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: CustomerTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("customer_type_id,customer_type_name,priority,token_prefix,is_default")] tblCustomerType tblCustomerType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblCustomerType.token_prefix = tblCustomerType.token_prefix.ToUpper();
                    dbCustomerType.Edit(tblCustomerType);
                    return RedirectToAction("Index");
                }
                return View(tblCustomerType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Edit/5
        public IActionResult SetDefault(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCustomerType tblCustomerType = dbCustomerType.GetById(id.Value);
                if (tblCustomerType == null)
                {
                    return NotFound();
                }
                tblCustomerType.token_prefix = tblCustomerType.token_prefix.ToUpper();
                tblCustomerType.is_default = 1;
                dbCustomerType.Edit(tblCustomerType);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: CustomerTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblCustomerType tblCustomerType = dbCustomerType.GetById(id.Value);
                if (tblCustomerType == null)
                {
                    return NotFound();
                }
                return View(tblCustomerType);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: CustomerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                dbCustomerType.Remove(id);
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
