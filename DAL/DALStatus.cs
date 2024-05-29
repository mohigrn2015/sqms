using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALStatus
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_Status_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALStatus",
                    procedure_name = "USP_Status_SelectAll",
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
