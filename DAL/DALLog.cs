using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;

namespace SQMS.DAL
{
    public class DALLog
    {
        MySQLManager manager;
        public int ApiRequestLogInsert(ApiRequestLog requestLog)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_loginprovider", requestLog.loginprovider));
                manager.AddParameter(new MySqlParameter("p_methode_name", requestLog.methode_name));
                manager.AddParameter(new MySqlParameter("p_request_json", requestLog.request_json));
                manager.AddParameter(new MySqlParameter("p_response_json", requestLog.response_json));
                
                long? log_id = manager.CallStoredProcedure_Insert("USP_APIRequestLog_Insert");
                
                if (log_id.HasValue) return (int)log_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALLog",
                    procedure_name = "USP_APIRequestLog_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public int SignalRBroadcastLogInsert(SignalRBroadcastLog broadcastLog)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", broadcastLog.branch_id));
                manager.AddParameter(new MySqlParameter("p_counter_no", broadcastLog.counter_no));
                manager.AddParameter(new MySqlParameter("p_token_no", broadcastLog.token_no));
                manager.AddParameter(new MySqlParameter("p_is_token_added", broadcastLog.is_token_added));
                manager.AddParameter(new MySqlParameter("p_is_token_called", broadcastLog.is_token_called));
                manager.AddParameter(new MySqlParameter("p_is_playlist_changed", broadcastLog.is_playlist_changed));
                manager.AddParameter(new MySqlParameter("p_is_footer_changed", broadcastLog.is_footer_changed));
                long? log_id = manager.CallStoredProcedure_Insert("USP_SignalRBroadcastLog_Insert");
                if (log_id.HasValue) return (int)log_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALLog",
                    procedure_name = "USP_SignalRBroadcastLog_Insert",
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
