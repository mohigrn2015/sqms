using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Globalization;

namespace SQMS.Controllers
{
    [AuthorizationFilter]
    public class NotificationsController : Controller
    {
        private readonly BLLNotification dbManager = new BLLNotification();
        private readonly BLLBranchUsers dbBranchUser = new BLLBranchUsers();
        private readonly BLLBranch dbBranch = new BLLBranch();
        private readonly notifyDisplay notifyDisplay;
        private readonly IHttpContextAccessor _session;

        public NotificationsController(notifyDisplay _notifyDisplay, IHttpContextAccessor session)
        {
            this.notifyDisplay = _notifyDisplay;
            _session = session;
        }
        // GET: Notification
        [RightPrivilegeFilter(PageIds = 800718)]
        public IActionResult Index()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                return View(dbManager.GetAllNotification(sm.user_id));
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Notification/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Notification/Create
        [RightPrivilegeFilter(PageIds = 800719)]
        public IActionResult Create()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (sm.branch_id > 0)
                {
                    var branches = dbBranch.GetBranchesByUserId(sm.user_id).ToList();
                    ViewBag.branchList = branches;
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower() && branches.Any(y => y.branch_id == x.branch_id)).ToList();
                }
                else
                {
                    ViewBag.branchList = dbBranch.GetAllBranch();
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower() && x.branch_id > 0).ToList();
                }

                ViewBag.userBranchId = sm.branch_id;

                return View();
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: Notification/Create
        [HttpPost]
        public async Task<IActionResult> Create(tblNotification notification)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (!notification.sent_now)
                {
                    string dateAndTime = notification.NotificationDate + ' ' + notification.NotificationTime; // 05/25/1987 10:30PM
                    DateTime dt = DateTime.Parse(dateAndTime);
                    notification.notification_date_time = dt; //"2022-03-23 9:00 AM"
                }

                if (!notification.sent_now && notification.notification_date_time < DateTime.Now)
                {

                    ViewBag.branchList = dbBranch.GetAllBranch();
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower()).ToList();
                    notification.is_PostBack = true;
                    ModelState.AddModelError("", "Notification date time expired.");
                    return View(notification);
                }

                notification.created_by = sm.user_id;
                byte[] fileData = null;

                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        Request.Form.Files[0].CopyTo(stream);
                        notification.NotificationFile = stream.ToArray();
                    }
                    notification.have_attachment = true;
                }

                notification.notification_id = dbManager.Create(notification);

                if (notification.sent_now)
                {
                   await notifyDisplay.SendNotification(notification.SelectedUserId, notification.message, notification.have_attachment, notification.notification_id);
                }
                else
                {
                    if (NotificationManager.ListOfNotfication == null)
                    {
                        NotificationManager.ListOfNotfication = new List<tblNotification>();
                    }
                    NotificationManager.ListOfNotfication.Add(notification);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Notification/Edit/5
        [RightPrivilegeFilter(PageIds = 800720)]
        public IActionResult Edit(int id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                tblNotification notification = dbManager.GetById(id);
                if (notification == null)
                {
                    return NotFound();
                }

                if (sm.branch_id > 0)
                {
                    var branches = dbBranch.GetBranchesByUserId(sm.user_id).ToList();
                    ViewBag.branchList = branches;
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower() && branches.Any(y => y.branch_id == x.branch_id)).ToList();
                }
                else
                {
                    ViewBag.branchList = dbBranch.GetAllBranch();
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower() && x.branch_id > 0).ToList();
                }

                ViewBag.userBranchId = sm.branch_id;
                return View(notification);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // POST: Notification/Edit/5

        //[HttpPost]
        //public IActionResult Edit(tblNotification notification)
        //{
        //    SessionManager sm = new SessionManager(_session);
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (!notification.sent_now)
        //            {
        //                string dateAndTime = notification.NotificationDate + ' ' + notification.NotificationTime; // 05/25/1987 10:30PM
        //                DateTime dt = DateTime.Parse(dateAndTime);
        //                notification.notification_date_time = dt; //"2022-03-23 9:00 AM"
        //            }
        //        }

        //        if (!notification.sent_now && notification.notification_date_time < DateTime.Now)
        //        {

        //            ViewBag.branchList = dbBranch.GetAllBranch();
        //            ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower()).ToList();
        //            notification.is_PostBack = true;
        //            ModelState.AddModelError("", "Notification date time expired.");
        //            return View(notification);
        //        }
        //        notification.updated_by = sm.user_id;

        //        byte[] fileData = null;
        //        if (Request.Form.Files[0] != null && Request.Form.Files[0].Length > 0)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                Request.Form.Files[0].CopyTo(memoryStream);
        //                fileData = memoryStream.ToArray();
        //            }

        //            notification.have_attachment = true;
        //            notification.NotificationFile = fileData;
        //        }

        //        // TODO: Add insert logic here
        //        dbManager.Update(notification);

        //        if (NotificationManager.ListOfNotfication == null)
        //        {
        //            NotificationManager.ListOfNotfication = new List<tblNotification>();
        //        }

        //        tblNotification oldNotification = NotificationManager.ListOfNotfication.Where(x => x.notification_id == notification.notification_id).FirstOrDefault();

        //        if (oldNotification != null && oldNotification.notification_id > 0)
        //        {
        //            NotificationManager.ListOfNotfication.Remove(oldNotification);
        //        }

        //        if (notification.sent_now)
        //        {
        //            notifyDisplay.SendNotification(notification.SelectedUserId, notification.message, notification.have_attachment, notification.notification_id);

        //        }
        //        else
        //        {
        //            NotificationManager.ListOfNotfication.Add(notification);
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        sm.error = ex;
        //        return RedirectToAction("Index", "ErrorHandler");
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(tblNotification notification)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                string dateAndTime = notification.NotificationDate + ' ' + notification.NotificationTime;
                DateTime dt2 = DateTime.Now;
                if (!String.IsNullOrEmpty(dateAndTime) && dateAndTime != " ") 
                {
                    dt2 = DateTime.Parse(dateAndTime);
                    notification.notification_date_time = dt2;
                }

                if (DateTime.TryParseExact(dateAndTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    notification.notification_date_time = dt2;
                }

                if (!notification.sent_now && notification.notification_date_time < DateTime.Now)
                {
                    ViewBag.branchList = dbBranch.GetAllBranch();
                    ViewBag.UserList = dbBranchUser.GetAll().Where(x => x.Name.ToLower() == ("Service Holder").ToLower()).ToList();
                    notification.is_PostBack = true;
                    ModelState.AddModelError("", "Notification date time expired.");
                    return View(notification);
                }
                    notification.updated_by = sm.user_id;

                byte[] fileData = null;
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    if (Request.Form.Files[0] != null && Request.Form.Files[0].Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Request.Form.Files[0].CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                        }

                        notification.have_attachment = true;
                        notification.NotificationFile = fileData;
                    }
                }

                notification.notification_id = dbManager.Update(notification);

                if (NotificationManager.ListOfNotfication == null)
                {
                    NotificationManager.ListOfNotfication = new List<tblNotification>();
                }

                tblNotification oldNotification = NotificationManager.ListOfNotfication.Where(x => x.notification_id == notification.notification_id).FirstOrDefault();

                if (oldNotification != null && oldNotification.notification_id > 0)
                {
                    NotificationManager.ListOfNotfication.Remove(oldNotification);
                }

                if (notification.sent_now)
                {
                   await notifyDisplay.SendNotification(notification.SelectedUserId, notification.message, notification.have_attachment, notification.notification_id);

                }
                else
                {
                    NotificationManager.ListOfNotfication.Add(notification);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        // GET: Notification/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notification/Delete/5
        [HttpPost]
        public IActionResult Delete(int id, FormCollection collection)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }


        public IActionResult ViewAttachment(int id)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                var byteData = dbManager.GetNotificationAttachment(id);

                if (byteData != null && byteData.Length > 0)
                {
                    return File(byteData.ToArray(), "application/pdf");
                }
                return RedirectToAction("Edit", "Notifications", new { id = id });
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        [HttpPost]
        public IActionResult SeenNotification(int notificationId)
        {
            try
            {
                SessionManager sm = new SessionManager(_session);
                bool result = dbManager.NotificationSeen(notificationId, sm.user_id);
                return Ok(new { Success = result });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
            }
        }


        [HttpGet]
        [ActionName("get-notification")]
        public IActionResult GetNotification(int id)
        {
            tblNotification notification = new tblNotification();
            try
            {

                notification = dbManager.GetById(id);


            }
            catch (Exception ex)
            {
                return Ok(new { data = notification });
            }
            return Ok(new { data = notification });
        }

        [HttpGet]
        [ActionName("users-notification")]
        public IActionResult UserNotification()
        {
            List<tblNotification> list = new List<tblNotification>();
            try
            {

                SessionManager sm = new SessionManager(_session);
                list = dbManager.GetNotificationByUserId(sm.user_id);

            }
            catch (Exception ex)
            {
                return Ok(new { data = list });
            }
            return Ok(new { data = list });
        }

        [HttpGet]
        [ActionName("recent-users-notification")]
        public IActionResult RecentUserNotification()
        {
            List<tblNotification> list = new List<tblNotification>();
            int totalNotification = 0;
            try
            {

                SessionManager sm = new SessionManager(_session);

                var notificationList = dbManager.GetNotificationByUserId(sm.user_id);
                if (notificationList != null)
                {
                    totalNotification = notificationList.Where(x => !x.is_seen).Count();
                    list = notificationList.OrderBy(x => x.is_seen).ThenByDescending(x => x.notification_date_time).Take(3).ToList();
                }

            }
            catch (Exception ex)
            {
                return Ok(new { data = list, unseen = totalNotification });
            }
            return Ok(new { data = list, unseen = totalNotification });
        }

        [HttpGet]
        [ActionName("scheduled-notification")]
        public IActionResult ScheduledNotification()
        {
            bool isSuccess = false;
            try
            {
                SessionManager sm = new SessionManager(_session);
                tblNotification notification = new tblNotification();

                if (NotificationManager.ListOfNotfication == null)
                {
                    NotificationManager.ListOfNotfication = new List<tblNotification>();
                }

                notification = NotificationManager.ListOfNotfication.Where(x => x.notification_date_time.ToString("dd/MM/yyyy hh:mmtt") == DateTime.Now.ToString("dd/MM/yyyy hh:mmtt") && x.SelectedUserId.Contains(sm.user_id)).FirstOrDefault();

                if (notification != null && notification.notification_id > 0)
                {
                    notifyDisplay.SendNotification(notification.SelectedUserId, notification.message, notification.have_attachment, notification.notification_id);
                    NotificationManager.ListOfNotfication.Remove(notification);
                }

                List<tblNotification> oldNotificationInList = NotificationManager.ListOfNotfication.Where(x => x.notification_date_time <= DateTime.Now).ToList();
                List<tblNotification> result = NotificationManager.ListOfNotfication.Except(oldNotificationInList).ToList();
                NotificationManager.ListOfNotfication = result;

                isSuccess = true;
            }
            catch (Exception ex)
            {
                return Ok(new { result = isSuccess });
            }
            return Ok(new { result = isSuccess });
        }
    }
}
