using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Branch Admin, Service Holder")]
    public class CustomersController : Controller
    {
        private readonly BLLCustomer _dbManager = new BLLCustomer();
        private readonly BLLCustomerType _dbCustomerType = new BLLCustomerType();
        private readonly IHttpContextAccessor _session;
        public CustomersController(IHttpContextAccessor session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            try
            {
                return View(_dbManager.GetAll());
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tblCustomer tblCustomer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbManager.Create(tblCustomer);
                    return RedirectToAction("Index");
                }
                return View(tblCustomer);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var tblCustomer = _dbManager.GetById(id.Value);
                if (tblCustomer == null)
                {
                    return NotFound();
                }

                ViewBag.type_id = _dbCustomerType.GetAll();
                //ViewBag.type_id = new SelectList(_dbCustomerType.GetAll(), "Customer_type_id", "CUSTOMER_TYPE_NAME", tblCustomer.customer_type_id);
                return View(tblCustomer);
            }
            catch (Exception ex)
            {
                SessionManager sm = new SessionManager(_session);
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(tblCustomer tblCustomer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbManager.Edit(tblCustomer);
                    return RedirectToAction("Index");
                }
                ViewBag.type_id = _dbCustomerType.GetAll();
                //ViewBag.type_id = new SelectList(_dbCustomerType.GetAll(), "Customer_type_id", "CUSTOMER_TYPE_NAME", tblCustomer.customer_type_id);
                return View(tblCustomer);
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
