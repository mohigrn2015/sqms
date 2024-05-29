using MySql.Data.MySqlClient;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALReport
    {
        MySQLManager manager;
        public DataTable LocalCustomerReport(int branch_id, string user_id, string login_user_id, int counter_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_customer_type_id", customer_type_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                {
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                }
                else
                {
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));
                }

                return manager.CallStoredProcedure_Select("RSP_LocalCustomer");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_LocalCustomer",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable BreakReport(int branch_id, string user_id, string login_user_id, int counter_id, int break_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_break_type_id", break_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_BrekReport");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_BrekReport",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable AgentWiseSummaryReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_AGENTWISESUMMURY");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_AGENTWISESUMMURY",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable CentreWiseSummaryReport(int branch_id, string login_user_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_CENTREWISESUMMARYREPORT");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_CENTREWISESUMMARYREPORT",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GeneralSearchReport(int branch_id, string user_id, string login_user_id, int counter_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_customer_msisdn_no", msisdn_no));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_token_no", token_no));
                manager.AddParameter(new MySqlParameter("p_padding_left", ApplicationSetting.PaddingLeft));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", null));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_GENERALSEARCH_V8");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_GENERALSEARCH_V8",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable ServiceSummaryReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_ServiceSummary");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_ServiceSummary",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable SingleVSMultipleVisitedReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_SingleVSMultipleVisited");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_SingleVSMultipleVisited",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable TokenExceedingReport(int branch_id, string user_id, string login_user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_flag", flag));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_TOKENEXCEEDING");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_TOKENEXCEEDING",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable TopNServiceReport(int branch_id, string user_id, string login_user_id, int counter_id, DateTime start_date, DateTime end_date, int topN)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_top_n", topN));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_TopNService");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_TopNService",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable LogoutDetailReport(int branch_id, string user_id, string login_user_id, int counter_id, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id == null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_LOGOUTDETAILS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_LOGOUTDETAILS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable LoginAttemptDetailsReport(int branch_id, string user_id, string login_user_id, int counter_id, int is_success, DateTime start_date, DateTime end_date)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_user_id", user_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));
                manager.AddParameter(new MySqlParameter("p_is_success", is_success));
                manager.AddParameter(new MySqlParameter("p_start_date", start_date.ToString("yyyy-MM-dd HH:mm:ss")));
                manager.AddParameter(new MySqlParameter("p_end_date", end_date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (login_user_id != null)
                    manager.AddParameter(new MySqlParameter("p_login_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_login_user_id", login_user_id));

                return manager.CallStoredProcedure_Select("RSP_LOGINATTEMPTDETAILS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALReport",
                    procedure_name = "RSP_LOGINATTEMPTDETAILS",
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
