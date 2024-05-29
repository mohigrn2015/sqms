using MySql.Data.MySqlClient;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALBranchDisplayFooter
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();   
            try
            {
                return manager.CallStoredProcedure_Select("USP_Branch_DF_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchDisplayFooter",
                    procedure_name = "USP_Branch_DF_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetById(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_display_footer_id", id));

                return manager.CallStoredProcedure_Select("USP_Branch_DF_SelectById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchDisplayFooter",
                    procedure_name = "USP_Branch_DF_SelectById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Insert(VMBranchDisplayFooter displayFooter)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(displayFooter);
                long? display_footer_id = manager.CallStoredProcedure_Insert("USP_Branch_DF_Insert");
                if (display_footer_id.HasValue) return (int)display_footer_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchDisplayFooter",
                    procedure_name = "USP_Branch_DF_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void Update(VMBranchDisplayFooter displayFooter)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_display_footer_id", displayFooter.branch_display_footer_id));
                MapParameters(displayFooter);
                manager.CallStoredProcedure_Update("USP_Branch_DF_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchDisplayFooter",
                    procedure_name = "USP_Branch_DF_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        private void MapParameters(VMBranchDisplayFooter displayFooter)
        {
            manager.AddParameter(new MySqlParameter("p_branch_id", displayFooter.branch_id));
            manager.AddParameter(new MySqlParameter("p_display_footer_id", displayFooter.display_footer_id)); ;
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_display_footer_id", id));

                manager.CallStoredProcedure_Update("USP_Branch_DF_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchDisplayFooter",
                    procedure_name = "USP_Branch_DF_Delete",
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
