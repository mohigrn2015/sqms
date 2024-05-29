using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Helper;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Net;

namespace SQMS.Controllers
{
    public class CounterCustomerTypesController : Controller
    {
        private readonly BLLCounterCustomerTypes dbManager = new BLLCounterCustomerTypes();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly BLLCustomerType dbCustomerType = new BLLCustomerType();
        private readonly BLLCounters dbCounter = new BLLCounters();
        private readonly IHttpContextAccessor _session;
        public CounterCustomerTypesController(IHttpContextAccessor session)
        {
            _session = session;
        }

        // GET: CounterCustomerTypes
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800715)]
        public IActionResult Index()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                ViewBag.userBranchId = sm.branch_id;
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Counters/Create
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800716)]
        public IActionResult Create()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                int branch_id = sm.branch_id;
                //ViewBag.branch_id = dbBranch.GetAllBranch();
                ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", branch_id);
                ViewBag.customer_type_id = dbCustomerType.GetAll();
                //ViewBag.customer_type_id = new SelectList(dbCustomerType.GetAll(), "customer_type_id", "customer_type_name");
                //ViewBag.counter_id = dbCounter.GetCounterByBrunchId(branch_id);
                return View();
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        // POST: Counters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        ////[ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult Create(VMCounterCustomerType counterCustType)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    counterCustType.branch_id = new SessionManager(_session).branch_id;
                }
                else
                {
                    counterCustType.branch_id = counterCustType.branch_id;
                }


                if (counterCustType.customer_type_id != 0 && counterCustType.branch_id != 0)
                {
                    dbManager.Create(counterCustType);

                    return RedirectToAction("Index");
                }

                ViewBag.branch_id = dbBranch.GetAllBranch();
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name");
                ViewBag.customer_type_id = dbCustomerType.GetAll();
                //ViewBag.customer_type_id = new SelectList(dbCustomerType.GetAll(), "customer_type_id", "customer_type_name");

                return View(counterCustType);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Counters/Edit/5
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800717)]
        public IActionResult Edit(int? id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                VMCounterCustomerType counterCustomerType = dbManager.GetById(id.Value);
                if (counterCustomerType == null)
                {
                    return NotFound();
                }

                //ViewBag.branch_id = dbBranch.GetAllBranch();
                ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", counterCustomerType.branch_id);
                //ViewBag.counter_id = dbCounter.GetCounterByBrunchId(counterCustomerType.branch_id);
                ViewBag.counter_id = new SelectList(dbCounter.GetCounterByBrunchId(counterCustomerType.branch_id), "counter_id", "counter_no", counterCustomerType.counter_id);
                //ViewBag.customer_type_id = dbCustomerType.GetAll();
                ViewBag.customer_type_id = new SelectList(dbCustomerType.GetAll(), "customer_type_id", "customer_type_name", counterCustomerType.counter_customer_type_id);

                return View(counterCustomerType);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: Counters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult Edit(VMCounterCustomerType counterCustomerType)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (counterCustomerType.counter_customer_type_id != 0)
                {
                    dbManager.Edit(counterCustomerType);

                    return RedirectToAction("Index");
                }
                return View(counterCustomerType);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult Activate(int? id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                VMCounterCustomerType counterCustomerType = dbManager.GetById(id.Value);

                if (counterCustomerType == null)
                {
                    return NotFound();
                }
                counterCustomerType.is_active = 1;
                dbManager.ActiveOrDeactive(counterCustomerType);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult Deactivate(int? id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                VMCounterCustomerType counterCustomerType = dbManager.GetById(id.Value);

                if (counterCustomerType == null)
                {
                    return NotFound();
                }
                counterCustomerType.is_active = 0;
                dbManager.ActiveOrDeactive(counterCustomerType);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
    }
}