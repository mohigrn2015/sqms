using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Utility;
using System.Net;
using SQMS.Models;
using Microsoft.AspNetCore.Http;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Branch Admin")]
    public class BranchesController : Controller
    {
        private readonly BLLBranch dbManager = new BLLBranch();
        private readonly BLLServiceDetail dbService = new BLLServiceDetail();
        private readonly IHttpContextAccessor _session;


        public BranchesController(IHttpContextAccessor session)
        {
            _session = session;
        }

        // GET: Branches
        //public async Task<IActionResult> Index()
        [RightPrivilegeFilter(PageIds = 800740)]
        public IActionResult Index()
        {
            try
            {
                return View(dbManager.GetAllBranch());
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Branches
        //public async Task<IActionResult> Index()
        [AuthorizationFilter(Roles = "Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800721)]
        public IActionResult CountersStatus()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                ViewBag.branch_id = sm.branch_id;
                return View(dbManager.GetCounterCurrentStatus(sm.branch_id, 0));
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        //public async Task<IActionResult> Index()
        [AuthorizationFilter(Roles = "Branch Admin")]
        public IActionResult GetCounterCurrentStatus()
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                ViewBag.branch_id = sm.branch_id;
                var CounterStatusList = dbManager.GetCounterCurrentStatus(sm.branch_id, 0);
                return Ok(new { Success = true, counterStatusList = CounterStatusList });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = ex.Message });

            }

        }

        // GET: Branches/Details/5
        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblBranch tblBranch = dbManager.GetById(id.Value);
                if (tblBranch == null)
                {
                    return NotFound();
                }
                return View(tblBranch);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Create
        [RightPrivilegeFilter(PageIds = 800741)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("branch_id,branch_name,address,contact_person,contact_no,display_next,static_ip")] tblBranch tblBranch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(tblBranch);
                    return RedirectToAction("Index");
                }
                return View(tblBranch);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: Branches/Edit/5
        [RightPrivilegeFilter(PageIds = 800742)]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblBranch tblBranch = dbManager.GetById(id.Value);
                if (tblBranch == null)
                {
                    return NotFound();
                }
                return View(tblBranch);
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
        public IActionResult Edit([Bind("branch_id,branch_name,address,contact_person,contact_no,display_next,static_ip")] tblBranch tblBranch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblBranch);

                    return RedirectToAction("Index");
                }
                return View(tblBranch);
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
                tblBranch tblBranch = dbManager.GetById(id.Value);
                if (tblBranch == null)
                {
                    return NotFound();
                }
                return View(tblBranch);
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
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    tblBranch tblBranch = dbManager.GetById(id);
        //    db.tblBranches.Remove(tblBranch);

        //    return RedirectToAction("Index");
        //}
        //--------------- Edit ----------------
        public IActionResult AutocompleteBranchSuggestions(string term)
        {
            try
            {
                //   var suggestions = unitOfWork.EmployeesRepository.Get().Where(w => w.IdentificationNumber.ToLower().Trim().Contains(term.ToLower().Trim()) && w.OCode == OCode && w.PFStatus != 2).OrderBy(s => s.IdentificationNumber).Select(s => new { value = s.EmpName, label = s.IdentificationNumber }).ToList();
                //List<tblBranch> branchList = new List<tblBranch>();
                var branchList = dbManager.GetAllBranch().Where(x => x.branch_name.ToLower().Trim().Contains(term.ToLower().Trim())).Select(s => new { value = s.branch_id, label = s.branch_name }).ToList();
                return Ok(branchList);
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = ex.Message });
            }
        }

    }
}
