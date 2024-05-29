using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;
using System.Diagnostics;

namespace SQMS.DAL
{
    public class DALToken
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();   
            try
            {
                return manager.CallStoredProcedure_Select("USP_Token_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_Token_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetBy(DateTime from_date, DateTime to_date, int token_id = 0, int branch_id = 0, int service_type_id = 0, int service_status_id = 0,
            int counter_id = 0, string user_id = null, string token_prefix = null, string contact_no = null)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_from_date", Value = from_date });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_to_date", Value = to_date });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_token_id", Value = token_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_branch_id", Value = branch_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_service_type_id", Value = service_type_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_service_status_id", Value = service_status_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_counter_id", Value = counter_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_user_id", Value = user_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_token_prefix", Value = token_prefix });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_contact_no", Value = contact_no });

                return manager.CallStoredProcedure_Select("USP_Token_SelectBy");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_Token_SelectBy",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetSkipped(int? branch_id, string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_branch_id", Value = branch_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USER_ID", Value = user_id });

                return manager.CallStoredProcedure_Select("USP_TOKEN_SelectSkipped");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_TOKEN_SelectSkipped",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetByBranchId(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_branch_id", Value = branch_id });

                return manager.CallStoredProcedure_Select("USP_TOKEN_SelectByBranchId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_TOKEN_SelectByBranchId",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public string GetNextTokenList(int branch_id)
        {
            manager = new MySQLManager();
            string param_NEXT_TOKENS = string.Empty;
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_padding_left", ApplicationSetting.PaddingLeft));
      
                DataTable dt = manager.CallStoredProcedure_Select("USP_GetNextTokenList");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) 
                    {
                        param_NEXT_TOKENS = (dr["PO_NEXT_TOKENS"] == DBNull.Value ? "" : dr["PO_NEXT_TOKENS"].ToString());
                    }
                }

                return param_NEXT_TOKENS;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_GetNextTokenList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetProgressTokenList(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));

                return manager.CallStoredProcedure_Select("USP_GetInProgressTokenList");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_GetInProgressTokenList",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable Insert(tblTokenQueue token)
        {
            manager = new MySQLManager();
            DataTable dt = new DataTable();
            try
            {                
               MapParameters(token);
                
               dt = manager.CallStoredProcedure_TokenInsert("USP_Token_Insert");

            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_Token_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
            return dt;
        }

        public void ReInitiate(long token_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_token_id", token_id));
                manager.CallStoredProcedure("USP_TOKEN_RE_INITIATE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_TOKEN_RE_INITIATE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void AssignToMe(long token_id, int counter_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_token_id", token_id));
                manager.AddParameter(new MySqlParameter("P_counter_id", counter_id));

                manager.CallStoredProcedure("USP_TOKEN_RE_ASSIGNTOME");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALToken",
                    procedure_name = "USP_TOKEN_RE_ASSIGNTOME",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        private void MapParameters(tblTokenQueue token)
        {
            manager.AddParameter(new MySqlParameter("p_branch_id", token.branch_id));
            manager.AddParameter(new MySqlParameter("p_service_type_id", token.service_type_id));
            manager.AddParameter(new MySqlParameter("p_contact_no", token.contact_no));
            manager.AddParameter(new MySqlParameter("p_default_token_prefix", token.token_prefix));
        }
    }
}
