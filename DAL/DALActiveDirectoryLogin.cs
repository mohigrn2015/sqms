using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALActiveDirectoryLogin
    {
        MySQLManager manager;
        public DataTable GetUserInfoByLoginName(string loginName)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_name", loginName));

                return manager.CallStoredProcedure_Select("USP_GET_USER_BY_NAME");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserInfoByLoginName",
                    procedure_name = "USP_GET_USER_BY_NAME",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        } 

        public DataTable GetUserInformation(string loginName)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_name", loginName));

                return manager.CallStoredProcedure_Select("USP_GET_USERINFORMATION");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserInformation",
                    procedure_name = "USP_GET_USERINFORMATION",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetUserInformationById(string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));

                return manager.CallStoredProcedure_Select("USP_GET_USERINFORMATIONBYID");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserInformationById",
                    procedure_name = "USP_GET_USERINFORMATIONBYID",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetUserRoleId(string roleName)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_role_name", roleName));

                return manager.CallStoredProcedure_Select("USP_GET_ROLE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserRoleId",
                    procedure_name = "USP_GET_ROLE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable Update_userPas(IdentityUser user)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_ID", user.Id));
                manager.AddParameter(new MySqlParameter("P_PASSWORDHASH", user.PasswordHash));
                manager.AddParameter(new MySqlParameter("P_SECURITYSTAMP", user.SecurityStamp));
                manager.AddParameter(new MySqlParameter("P_USERNAME", user.UserName));

                return manager.CallStoredProcedure_Select("USP_UpdateUserPas");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "Update_userPas",
                    procedure_name = "USP_UpdateUserPas",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable Change_userPas(ChangePassReqModel user)
        {
            manager = new MySQLManager();
            try
            {
                string changeTime = DateTime.Now.ToString();
                string timeChange = Cryptography.Encrypt(changeTime, true);
                manager.AddParameter(new MySqlParameter("P_ID", user.userId));
                manager.AddParameter(new MySqlParameter("P_CUR_PASS", user.currPass));
                manager.AddParameter(new MySqlParameter("P_PASSWORDHASH", user.newPass));
                manager.AddParameter(new MySqlParameter("P_SECURITYSTAMP", user.sequrityStamp));
                manager.AddParameter(new MySqlParameter("P_USERNAME", user.userName));
                manager.AddParameter(new MySqlParameter("P_CHANGETIME", timeChange));

                return manager.CallStoredProcedure_Select("USP_CHANGE_PASSWORD");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "Change_userPas",
                    procedure_name = "USP_CHANGE_PASSWORD",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable INSERT_USERS(IdentityUser userInfo, string HomeTown, int ForceChangeConfirmed, int isActiveDerectoryUser)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_ID", userInfo.Id));
                manager.AddParameter(new MySqlParameter("P_HOMETOWN", HomeTown));
                manager.AddParameter(new MySqlParameter("P_EMAIL", userInfo.Email));
                manager.AddParameter(new MySqlParameter("P_EMAILCONFIRMED", userInfo.EmailConfirmed));
                manager.AddParameter(new MySqlParameter("P_PASSWORDHASH", userInfo.PasswordHash));
                manager.AddParameter(new MySqlParameter("P_SECURITYSTAMP", userInfo.SecurityStamp));
                manager.AddParameter(new MySqlParameter("P_PHONE_NUMBER", userInfo.PhoneNumber));
                manager.AddParameter(new MySqlParameter("P_PHONE_NUMBERCONFIRMED", userInfo.PhoneNumberConfirmed));
                manager.AddParameter(new MySqlParameter("P_TWOFACTOR_ENABLE", userInfo.TwoFactorEnabled));
                manager.AddParameter(new MySqlParameter("P_LOCKOUTENDDATEUTC", userInfo.LockoutEnd));
                manager.AddParameter(new MySqlParameter("P_LOCKOUT_ENABLE", userInfo.LockoutEnabled));
                manager.AddParameter(new MySqlParameter("P_ACCESSFAILEDCOUNT", userInfo.AccessFailedCount));
                manager.AddParameter(new MySqlParameter("P_USERNAME", userInfo.UserName));
                manager.AddParameter(new MySqlParameter("P_FORCECHANGEDCONFIRMED", ForceChangeConfirmed));
                manager.AddParameter(new MySqlParameter("P_ISACTIVEDIRECTORY_USER", isActiveDerectoryUser));

                return manager.CallStoredProcedure_Select("USP_INSERTUSERS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "INSERT_USERS",
                    procedure_name = "USP_INSERTUSERS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable INSERT_Role(string user_id, string user_role)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_USER_ID", user_id));
                manager.AddParameter(new MySqlParameter("P_USER_ROLE", user_role));
               
                return manager.CallStoredProcedure_Select("USP_INSERTUSERS_ROLES");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "INSERT_Role",
                    procedure_name = "USP_INSERTUSERS_ROLES",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetRightInformationList(string loginName)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_LOGINNAME", loginName));
                
                return manager.CallStoredProcedure_Select("SQMS_GETPAGEPRIVELAGE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetRightInformationList",
                    procedure_name = "SQMS_GETPAGEPRIVELAGE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        internal UserInfoModel GetUserInfo(string userName)
        {
            manager = new MySQLManager();

            try
            {
                manager.AddParameter(new MySqlParameter("P_LOGIN_NAME", userName));

                DataSet userDataSet = manager.CallStoredProcedure_SelectDS("GSP_USERINFO");

                DataTable userDataTable = new DataTable();
                userDataTable = userDataSet.Tables[0];

                return (ConvertUserDataTableToModel(userDataTable));
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserInfo",
                    procedure_name = "GSP_USERINFO",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public UserInfoModel ConvertUserDataTableToModel(DataTable userData)
        {
            if (userData.Rows.Count > 0)
            {
                UserInfoModel userInfo = new UserInfoModel();
                DataRow row = userData.Rows[0];
                userInfo.center_id = Convert.ToInt32(row["currentcenterid"]);
                userInfo.current_center = row["centername"].ToString();
                userInfo.user_id = Convert.ToInt32(row["userid"]);
                userInfo.user_status = row["userstatus"].ToString();
                userInfo.is_locked = row["islocked"].ToString();
                userInfo.is_internal = row["isinternal"].ToString();
                userInfo.role_name = row["rolename"].ToString();
                userInfo.hash_password = row["password"].ToString();
                userInfo.latitude = Convert.ToDouble(row["lat"]);
                userInfo.longitude = Convert.ToDouble(row["lon"]);
                return userInfo;
            }
            return null;
        }
    }
}
