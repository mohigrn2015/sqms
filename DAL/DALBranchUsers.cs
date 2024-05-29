using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALBranchUsers
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_BRANCHUSERS_SELECTALL");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "USP_BRANCHUSERS_SELECTALL",
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
                manager.AddParameter(new MySqlParameter("p_branchuser_id", id));

                return manager.CallStoredProcedure_Select("USP_BranchUser_SelectList_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "USP_BranchUser_SelectList_ById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Insert(tblBranchUser branchuser)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(branchuser);

                long? branch_id = manager.CallStoredProcedure_Insert("USP_BranchUser_Insert");
                if (branch_id.HasValue) return (int)branch_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "USP_BranchUser_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void Update(tblBranchUser branchuser)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_user_branch_id", branchuser.user_branch_id));
                manager.AddParameter(new MySqlParameter("p_branch_id", branchuser.branch_id));
                manager.AddParameter(new MySqlParameter("p_transfer_by", branchuser.transfer_by));
                manager.AddParameter(new MySqlParameter("p_user_id", branchuser.user_id));
                manager.CallStoredProcedure_Update("USP_BRANCHUSER_UPDATE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "USP_BRANCHUSER_UPDATE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        private void MapParameters(tblBranchUser branchuser)
        {
            manager.AddParameter(new MySqlParameter("p_branch_id", branchuser.branch_id));
            manager.AddParameter(new MySqlParameter("p_user_id", branchuser.user_id));
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branchuser_id", id));

                manager.CallStoredProcedure_Delete("USP_BRANCHUSER_DELETE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "USP_BRANCHUSER_DELETE",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void SyncUser()
        {
            manager = new MySQLManager();
            try
            {
                manager.CallStoredProcedure_Insert("ISP_MIGRATION_USER");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchUsers",
                    procedure_name = "ISP_MIGRATION_USER",
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
