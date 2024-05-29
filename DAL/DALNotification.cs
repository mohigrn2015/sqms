using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALNotification
    {
        MySQLManager manager;
        public DataTable GetAllUpcommingNotificationOnAppStart()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_UPCOMMING_NOTIFICATIONS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_UPCOMMING_NOTIFICATIONS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetAllNotification(string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));

                return manager.CallStoredProcedure_Select("USP_NOTIFICATION_SELECTALL");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_SELECTALL",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetCurrentScheduleNotification()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_SCHEDULE_NOTIFICATION_ALL");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_SCHEDULE_NOTIFICATION_ALL",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void NotificationSeen(int notificationId, string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));
                manager.AddParameter(new MySqlParameter("p_notification_id", notificationId));
                manager.CallStoredProcedure_Update("USP_NOTIFICATION_SEEN");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_SEEN",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public int Insert(tblNotification notification)
        {
            manager = new MySQLManager();
            int notifyId = 0;
            try
            {
                MapParameters(notification);
                manager.AddParameter(new MySqlParameter("P_FILES_BYTE", notification.NotificationFile));

                DataTable dt = manager.CallStoredProcedure_Select("USP_NOTIFICATION_INSERT");

                foreach (DataRow row in dt.Rows)
                {
                    notifyId = Convert.ToInt32(row["po_PKValue"] == DBNull.Value ? 0 : row["po_PKValue"]);

                }
                return notifyId;
                ///long? notification_id = manager.CallStoredProcedure_Insert("USP_NOTIFICATION_INSERT");
                //if (notification_id.HasValue) return (int)notification_id.Value;
                //else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_INSERT",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// P_NOTIFICATION_ID
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>

        public int Update(tblNotification notification)
        {
            manager = new MySQLManager();
            int notifyId = 0;
            try
            {
                MapParametersUpdate(notification);
                manager.AddParameter(new MySqlParameter("P_FILES_BYTE", notification.NotificationFile));

                DataTable dt = manager.CallStoredProcedure_Select("USP_NOTIFICATION_UPDATE");

                foreach (DataRow row in dt.Rows)
                {
                    notifyId = Convert.ToInt32(row["P_NOTIFICATION_ID"] == DBNull.Value ? 0 : row["P_NOTIFICATION_ID"]);
                }
                return notifyId;

                //long? notification_id = manager.CallStoredProcedure_Insert("USP_NOTIFICATION_UPDATE");
                //if (notification_id.HasValue) return (int)notification_id.Value;
                //else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_UPDATE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetNotifyUserByNotificationId(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_notification_id", id));

                return manager.CallStoredProcedure_Select("USP_NOTIFY_USERS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFY_USERS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetById(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_notification_id", id));

                return manager.CallStoredProcedure_Select("USP_NOTIFICATION_BYID");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_BYID",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetNotificationAttachment(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_notification_id", id));

                return manager.CallStoredProcedure_Select("USP_NOTIFICATION_ATTACHMENT");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_ATTACHMENT",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetNotificationByUserId(string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));

                return manager.CallStoredProcedure_Select("USP_NOTIFICATION_LIST_BYUSER");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_NOTIFICATION_LIST_BYUSER",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetUnSeenNotificationByUserId(string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));

                return manager.CallStoredProcedure_Select("USP_UNSEEN_NOTIFICATION_LIST");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALNotification",
                    procedure_name = "USP_UNSEEN_NOTIFICATION_LIST",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        private void MapParameters(tblNotification notification)
        {
            manager.AddParameter(new MySqlParameter("P_SENT_NOW", (notification.sent_now) ? 1 : 0));
            manager.AddParameter(new MySqlParameter("P_NOTIFICATION_DATE_TIME", notification.notification_date_time));
            manager.AddParameter(new MySqlParameter("P_MESSAGE", notification.message));
            manager.AddParameter(new MySqlParameter("P_HAVE_ATTACHMENTS", (notification.have_attachment) ? 1 : 0));
            manager.AddParameter(new MySqlParameter("P_CREATED_BY", notification.created_by));
            manager.AddParameter(new MySqlParameter("P_SELECTED_USERS", notification.SelectedUserId));
            //manager.AddParameter(new OracleParameter("P_FILES_BYTE", notification.NotificationFile));
        }

        private void MapParametersUpdate(tblNotification notification)
        {
            manager.AddParameter(new MySqlParameter("P_NOTIFICATION_ID", notification.notification_id));
            manager.AddParameter(new MySqlParameter("P_SENT_NOW", (notification.sent_now) ? 1 : 0));
            manager.AddParameter(new MySqlParameter("P_NOTIFICATION_DATE_TIME", notification.notification_date_time));
            manager.AddParameter(new MySqlParameter("P_MESSAGE", notification.message));
            manager.AddParameter(new MySqlParameter("P_HAVE_ATTACHMENTS", (notification.have_attachment) ? 1 : 0));
            manager.AddParameter(new MySqlParameter("P_UPDATED_BY", notification.created_by));
            manager.AddParameter(new MySqlParameter("P_SELECTED_USERS", notification.SelectedUserId));
            //manager.AddParameter(new OracleParameter("P_FILES_BYTE", notification.NotificationFile));
        }
    }
}
