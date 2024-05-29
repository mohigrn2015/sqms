using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALAspNetUser
    {
        MySQLManager manager;
        public DataTable GetAllUser()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_AspNetUser_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetAllUser",
                    procedure_name = "USP_AspNetUser_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetUserBySecurityCode(string securityToken)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_SecurityToken", securityToken));

                return manager.CallStoredProcedure_Select("USP_AspNetUser_SelectByToken");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetUserBySecurityCode",
                    procedure_name = "USP_AspNetUser_SelectByToken",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetLoginInfo(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));

                return manager.CallStoredProcedure_Select("USP_AspNetUserLogin_SelectById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetLoginInfo",
                    procedure_name = "USP_AspNetUserLogin_SelectById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void InsertLoginInfo(AspNetUserLogin loginInfo)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_LOGINPROVIDER", Value = loginInfo.LoginProvider });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_PROVIDERKEY", Value = loginInfo.ProviderKey });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USERID", Value = loginInfo.UserId });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_BRANCH_ID", Value = loginInfo.branch_id });

                manager.CallStoredProcedure_Insert("USP_AspNetUserLogin_Insert");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "InsertLoginInfo",
                    procedure_name = "USP_AspNetUserLogin_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetLoginAttemptsInfo(int branch_id, string user_id, int counter_id, int is_success, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_is_success", is_success));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date));

                return manager.CallStoredProcedure_Select("USP_AspNetUserLAttempts_Select");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetLoginAttemptsInfo",
                    procedure_name = "USP_AspNetUserLAttempts_Select",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void InsertLoginAttemptInfo(AspNetUserLoginAttempts loginAttempt)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_USERNAME", loginAttempt.UserName));
                manager.AddParameter(new MySqlParameter("P_LOGINPROVIDER", loginAttempt.LoginProvider));
                manager.AddParameter(new MySqlParameter("P_IS_SUCCESS", loginAttempt.is_success));
                manager.AddParameter(new MySqlParameter("P_IP_ADDRESS", loginAttempt.ip_address));
                manager.AddParameter(new MySqlParameter("P_MACHINE_NAME", loginAttempt.machine_name));

                manager.CallStoredProcedure_Insert("USP_AspNetUserLAttempts_Insert");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "InsertLoginAttemptInfo",
                    procedure_name = "USP_AspNetUserLAttempts_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void UpdateLoginInfo(string loginProvider, int counter_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_COUNTER_ID", Value = counter_id });

                manager.CallStoredProcedure_Delete("USP_AspNetUserLogin_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "UpdateLoginInfo",
                    procedure_name = "USP_AspNetUserLogin_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void UpdateBranchAdminLoginInfo(string loginProvider, int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_BRANCH_ID", Value = branch_id });

                manager.CallStoredProcedure_Delete("USP_BranchAdminLogin_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "UpdateBranchAdminLoginInfo",
                    procedure_name = "USP_BranchAdminLogin_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void UserForceChangeConfirmed(string user_id, bool isForceChangeConfirmed)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_forcechangeconfirmed", Value = (isForceChangeConfirmed ? 1 : 0) });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_FCConfirm");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "UserForceChangeConfirmed",
                    procedure_name = "USP_AspNetUser_FCConfirm",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void InsertPasswordInfo(string user_id, string passwordhash)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USERID", Value = user_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_CHANGETIMEHASH", Value = Cryptography.Encrypt(DateTime.Now.ToString(), true) });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_PASSWORDHASH", Value = passwordhash });

                manager.CallStoredProcedure_Insert("USP_AspNetUserPasswords_Insert");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "InsertPasswordInfo",
                    procedure_name = "USP_AspNetUserPasswords_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetPasswordInfo(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_USERID", user_id));

                return manager.CallStoredProcedure_Select("USP_AspNetUserPasswords_Select");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetPasswordInfo",
                    procedure_name = "USP_AspNetUserPasswords_Select",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void SetActivation(string user_id, int is_active)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_is_active", Value = is_active });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_SetActivation");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "SetActivation",
                    procedure_name = "USP_AspNetUser_SetActivation",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void SetActiveDirectoryUser(string user_id, int is_active_directory_user)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "p_is_active_directory_user", Value = is_active_directory_user });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_SetAD");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "SetActiveDirectoryUser",
                    procedure_name = "USP_AspNetUser_SetAD",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void UpdateLogoutInfo(string loginProvider, int? logout_type_id, string logoutReason)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                
                if (logout_type_id.HasValue)
                {
                    manager.AddParameter(new MySqlParameter() { ParameterName = "P_logout_type_id", Value = logout_type_id.Value });
                }
                else 
                {
                    manager.AddParameter(new MySqlParameter() { ParameterName = "P_logout_type_id", Value = DBNull.Value });
                }
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_logout_reason", Value = logoutReason });

                manager.CallStoredProcedure_Delete("USP_AspNetUserLogOut");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "UpdateLogoutInfo",
                    procedure_name = "USP_AspNetUserLogOut",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void DeleteLoginInfo(string loginProvider)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_loginProvider", Value = loginProvider });

                manager.CallStoredProcedure_Delete("USP_AspNetUserLogin_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DeleteLoginInfo",
                    procedure_name = "USP_AspNetUserLogin_Delete",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetSessionInfoByUserName(string userName)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_USER_NAME", userName));

                return manager.CallStoredProcedure_Select("USP_SessionInfo_ByUserName");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetSessionInfoByUserName",
                    procedure_name = "USP_SessionInfo_ByUserName",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void UpdateUserSetIdle(string loginProvider)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_loginProvider", Value = loginProvider });

                manager.CallStoredProcedure_Delete("USP_AspNetUserSetIdle");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "UpdateUserSetIdle",
                    procedure_name = "USP_AspNetUserSetIdle",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
