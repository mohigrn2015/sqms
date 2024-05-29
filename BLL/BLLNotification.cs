using SQMS.DAL;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLNotification
    {
        public List<tblNotification> GetAllUpcommingNotificationOnAppStart()
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetAllUpcommingNotificationOnAppStart();
            return ObjectMappingUpcommingList(dt);
        }

        public List<tblNotification> GetAllNotification(string userId)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetAllNotification(userId);
            return ObjectMappingList(dt);
        }

        public List<tblNotification> GetCurrentScheduleNotification()
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetCurrentScheduleNotification();
            return ObjectMappingListSchedule(dt);
        }

        public int Update(tblNotification notification)
        {
            DALNotification dal = new DALNotification();
            int notification_id = dal.Update(notification);
            notification.notification_id = notification_id;
            return notification_id;
        }

        public int Create(tblNotification notification)
        {
            DALNotification dal = new DALNotification();
            int notification_id = dal.Insert(notification);
            notification.notification_id = notification_id;
            return notification_id;
        }

        public tblNotification GetById(int id)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public byte[] GetNotificationAttachment(int id)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetNotificationAttachment(id);
            if (dt.Rows.Count > 0)
            {
                return (byte[])dt.Rows[0]["ATTACHMENTS"];
            }
            else return null;
        }

        public IList<string> GetNotifyUserByNotificationId(int id)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetNotifyUserByNotificationId(id);
            return NotificationUserMap(dt);
        }

        public List<tblNotification> GetNotificationByUserId(string userId)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetNotificationByUserId(userId);
            return ObjectMappingNotificationList(dt);
        }

        public List<tblNotification> GetUnSeenNotificationByUserId(string userId)
        {
            DALNotification dal = new DALNotification();
            DataTable dt = dal.GetUnSeenNotificationByUserId(userId);
            return ObjectMappingNotificationList(dt);
        }

        public bool NotificationSeen(int notificationId, string userId)
        {
            bool isSuccess = false;
            try
            {
                DALNotification dal = new DALNotification();
                dal.NotificationSeen(notificationId, userId);
                isSuccess = true;
            }
            catch (Exception ex)
            {

                throw;
            }

            return isSuccess;
        }

        internal List<string> NotificationUserMap(DataTable dt)
        {
            List<string> list = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string userId = (row["USER_ID"] == DBNull.Value ? null : row["USER_ID"].ToString()); ;
                list.Add(userId);

            }
            return list;
        }

        internal List<tblNotification> ObjectMappingList(DataTable dt)
        {
            List<tblNotification> list = new List<tblNotification>();
            foreach (DataRow row in dt.Rows)
            {
                tblNotification notification = new tblNotification();
                notification.notification_id = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);
                notification.sent_now = (row["SENT_NOW"].ToString() == "1" ? true : false);
                notification.have_attachment = (row["HAVE_ATTACHMENTS"].ToString() == "1" ? true : false);
                notification.message = (row["MESSAGE"] == DBNull.Value ? null : row["MESSAGE"].ToString());
                notification.notification_date_time = Convert.ToDateTime(row["notification_date_time"].ToString());
                notification.created_date = Convert.ToDateTime(row["created_date"].ToString());
                list.Add(notification);

            }
            return list;
        }
        internal List<tblNotification> ObjectMappingListSchedule(DataTable dt)
        {
            var notificationId = 0;
            List<tblNotification> list = new List<tblNotification>();
            foreach (DataRow row in dt.Rows)
            {
                notificationId = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);

                tblNotification notification = list.Where(n => n.notification_id == notificationId).FirstOrDefault();
                if (notification != null)
                {
                    notification.SelectedUserId += ", " + (row["USER_ID"] == DBNull.Value ? null : row["USER_ID"].ToString());
                }
                else
                {
                    notification = new tblNotification();
                    notification.notification_id = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);
                    notification.notification_date_time = Convert.ToDateTime(row["NOTIFICATION_DATE_TIME"].ToString());
                    notification.message = (row["MESSAGE"] == DBNull.Value ? null : row["MESSAGE"].ToString());
                    notification.have_attachment = (row["HAVE_ATTACHMENTS"].ToString() == "1" ? true : false);
                    notification.created_by = (row["CREATED_BY"] == DBNull.Value ? null : row["CREATED_BY"].ToString());
                    notification.SelectedUserId = (row["USER_ID"] == DBNull.Value ? null : row["USER_ID"].ToString());
                    list.Add(notification);
                }



            }
            return list;
        }
        internal List<tblNotification> ObjectMappingUpcommingList(DataTable dt)
        {
            List<tblNotification> list = new List<tblNotification>();
            foreach (DataRow row in dt.Rows)
            {
                tblNotification notification = new tblNotification();
                notification.notification_id = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);
                notification.sent_now = (row["SENT_NOW"].ToString() == "1" ? true : false);
                notification.have_attachment = (row["HAVE_ATTACHMENTS"].ToString() == "1" ? true : false);
                notification.message = (row["MESSAGE"] == DBNull.Value ? null : row["MESSAGE"].ToString());
                notification.notification_date_time = Convert.ToDateTime(row["notification_date_time"].ToString());
                notification.created_date = Convert.ToDateTime(row["created_date"].ToString());

                notification.SelectedUserId = string.Join(",", GetNotifyUserByNotificationId(notification.notification_id));

                list.Add(notification);

            }
            return list;
        }

        internal tblNotification ObjectMapping(DataRow row)
        {

            tblNotification notification = new tblNotification();
            notification.notification_id = Convert.ToInt32(row["ID"] == DBNull.Value ? 0 : row["ID"]);
            notification.message = (row["message"] == DBNull.Value ? null : row["message"].ToString());
            notification.sent_now = (row["SENT_NOW"].ToString() == "1" ? true : false);
            notification.have_attachment = (row["HAVE_ATTACHMENTS"].ToString() == "1" ? true : false);
            notification.notification_date_time = Convert.ToDateTime(row["notification_date_time"].ToString());
            //notification.NotificationFile = (byte[])row["ATTACHMENTS"];
            notification.SelectedUserId = string.Join(",", GetNotifyUserByNotificationId(notification.notification_id));
            //notification.SelectedUserId = Regex.Replace(notification.SelectedUserId, @"[^\u0000-\u007F]", string.Empty);
            return notification;
        }


        internal List<tblNotification> ObjectMappingNotificationList(DataTable dt)
        {
            List<tblNotification> list = new List<tblNotification>();
            foreach (DataRow row in dt.Rows)
            {
                tblNotification notification = new tblNotification();
                notification.notification_id = Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]);
                notification.sender = (row["sender"] == DBNull.Value ? null : row["sender"].ToString());
                notification.is_seen = (row["IS_SEEN"].ToString() == "1" ? true : false);
                notification.have_attachment = (row["HAVE_ATTACHMENTS"].ToString() == "1" ? true : false);
                notification.sent_now = (row["sent_now"].ToString() == "1" ? true : false);
                notification.message = (row["MESSAGE"] == DBNull.Value ? null : row["MESSAGE"].ToString());
                notification.notification_date_time = Convert.ToDateTime(row["notification_date_time"].ToString());
                notification.NotificationDate = Convert.ToDateTime(row["notification_date_time"].ToString()).ToString("dd/MM/yyyy hh:mmtt");
                list.Add(notification);
            }
            return list;
        }

    }
}
