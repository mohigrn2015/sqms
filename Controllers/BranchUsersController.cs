using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI;
using SQMS.Models.ViewModels;
using SQMS.Models;
using SQMS.Utility;
using System.Net;
using SQMS.BLL;
using SQMS.DAL;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Branch Admin")]
    public class BranchUsersController : Controller
    {
        private readonly BLLBranchUsers dbManager = new BLLBranchUsers();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly BLLAspNetUser dbUser = new BLLAspNetUser();
        private BLL.BLLUntagLog dbUntagLog = new BLL.BLLUntagLog();
        private readonly IHttpContextAccessor _session;

        public BranchUsersController(IHttpContextAccessor session)
        {
            _session = session; 
        }

        // GET: BranchUsers
        [RightPrivilegeFilter(PageIds = 800706)]
        public IActionResult Index()
        {
            SessionManager session = new SessionManager(_session);
            try
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                ViewBag.userBranchId = session.branch_id;
                return View(dbManager.GetAll());
            }
            catch (Exception ex)
            {
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: BranchUsers/Details/5


        // GET: BranchUsers/Create
        [AuthorizationFilter(Roles = "Admin")]
        [RightPrivilegeFilter(PageIds = 800735)]
        public IActionResult Create(string userId)
        {
            try
            {
                string user_id = Cryptography.Decrypt(userId, true);
                List<VMBranchLogin> branchUsers = dbManager.GetAll().Where(w => w.user_id == user_id).ToList();
                AspNetUser user = dbUser.GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
                if (user == null) return RedirectToAction("Index");

                ViewBag.branch_id = dbBranch.GetAllBranch().GroupJoin(branchUsers, br => br.branch_id, bu => bu.branch_id, (br, bu) => new { Branchs = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Branchs);
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch().GroupJoin(branchUsers, br => br.branch_id, bu => bu.branch_id, (br, bu) => new { Branchs = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Branchs), "branch_id", "branch_name");

                VMBranchLogin branchUser = branchUsers.FirstOrDefault();
                return View(branchUser);

            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: BranchUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin")]
        public IActionResult Create([Bind("user_id,UserName,branch_id")] VMBranchLogin BranchUser)
        {
            try
            {
                if (BranchUser.branch_id != 0 && !String.IsNullOrEmpty(BranchUser.user_id))
                {
                    tblBranchUser tblBranchUser = new tblBranchUser() { branch_id = BranchUser.branch_id, user_id = BranchUser.user_id };
                    dbManager.Create(tblBranchUser);
                    return RedirectToAction("Index");
                }
                List<VMBranchLogin> branchUsers = dbManager.GetAll().Where(w => w.user_id == BranchUser.user_id).ToList();
                AspNetUser user = dbUser.GetAllUser().Where(w => w.Id == BranchUser.user_id).FirstOrDefault();
                if (user == null) return RedirectToAction("Index");

                ViewBag.branch_id = dbBranch.GetAllBranch().GroupJoin(branchUsers, br => br.branch_id, bu => bu.branch_id, (br, bu) => new { Branchs = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Branchs);
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch().GroupJoin(branchUsers, br => br.branch_id, bu => bu.branch_id, (br, bu) => new { Branchs = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Branchs), "branch_id", "branch_name");


                return View();
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: BranchUsers/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblBranchUser tblBranchUser = dbManager.GetById(id.Value);
                if (tblBranchUser == null)
                {
                    return NotFound();
                }
                return View(tblBranchUser);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: BranchUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("user_branch_id,user_id,branch_id")] tblBranchUser tblBranchUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(tblBranchUser);
                    return RedirectToAction("Index");
                }
                return View(tblBranchUser);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // GET: BranchUsers/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                tblBranchUser tblBranchUser = dbManager.GetById(id.Value);
                if (tblBranchUser == null)
                {
                    return NotFound();
                }
                return View(tblBranchUser);
            }
            catch (Exception ex)
            {
                SessionManager session = new SessionManager(_session);
                session.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }

        // POST: BranchUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblBranchUser tblBranchUser = dbManager.GetById(id);
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

        public IActionResult UntagBranch(string _user_id, int _branch_id, string _transfer_by, int _userBranchId)
        {
            var branchLogin = new BLLBranchUsers().GetAll().Where(w => w.user_id == _user_id).ToList();

            tblUntagLog untagLog = new tblUntagLog()
            {
                user_id = _user_id,
                branch_id = _branch_id,
                transfer_by = _transfer_by
            };

            dbManager.Remove(_userBranchId);
            dbUntagLog.UntagLogCreate(untagLog);

            return Ok(new { success = "0", message = "Success" });
        }

        public IActionResult SyncUsers()
        {
            bool isSync = false;
            try
            {
                BLLBranchUsers branchUsers = new BLLBranchUsers();
                branchUsers.SyncUsers();
                isSync = true;
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(new { sync = isSync });
        }

        //[HttpPost]
        //public IActionResult SetActivationStatus(string user_id, int is_activate)
        //{
        //    try
        //    {
        //        new BLL.BLLAspNetUser().SetActivation(user_id, is_activate);
        //        return Json(new { success = "true", message = "Success" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = "false", message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }

        //}
    }
}
