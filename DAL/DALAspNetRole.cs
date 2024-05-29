using MySql.Data.MySqlClient;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALAspNetRole
    {
        MySQLManager manager;
        public DataTable GetAllRoles()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_AspNetRoles_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetAllRoles",
                    procedure_name = "USP_AspNetRoles_SelectAll",
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
