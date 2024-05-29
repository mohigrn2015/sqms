using MySql.Data.MySqlClient;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALAspNetUserRoles
    {
        MySQLManager manager;
        public DataTable GetAllUser()
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
                    method_name = "GetAllUser",
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
        public DataTable GetRolesByUserId(string userId)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_id", userId));
               
                return manager.CallStoredProcedure_Select("USP_AspNetRoles_SelectByUserId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetRolesByUserId",
                    procedure_name = "USP_AspNetRoles_SelectByUserId",
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
