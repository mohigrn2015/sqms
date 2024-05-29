using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALAgentTransferLog
    {
        MySQLManager manager = new MySQLManager();

        public int Insert(tblAgentTransferLog agentTransferLog)
        {
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", agentTransferLog.user_id));
                manager.AddParameter(new MySqlParameter("p_from_branch_id", agentTransferLog.from_branch_id));
                manager.AddParameter(new MySqlParameter("p_to_branch_id", agentTransferLog.to_branch_id));
                manager.AddParameter(new MySqlParameter("p_transfer_by", agentTransferLog.transfer_by));

                long? id = manager.CallStoredProcedure_Insert("USP_UNTAG_LOG_INSERT");
                if (id.HasValue) return (int)id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALAgentTransferLog",
                    procedure_name = "USP_UNTAG_LOG_INSERT",
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
