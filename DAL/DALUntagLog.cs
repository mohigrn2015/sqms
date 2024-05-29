using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;

namespace SQMS.DAL
{
    public class DALUntagLog
    {
        MySQLManager manager;
        public int Insert(tblUntagLog untagLog)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", untagLog.user_id));
                manager.AddParameter(new MySqlParameter("p_branch_id", untagLog.branch_id));
                manager.AddParameter(new MySqlParameter("p_transfer_by", untagLog.transfer_by));

                long? id = manager.CallStoredProcedure_Insert("USP_UNTAG_LOG_INSERT");
                if (id.HasValue) return (int)id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALUntagLog",
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
