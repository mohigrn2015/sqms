using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALBranch
    {
        MySQLManager manager;
        public DataTable GetAllBranch()
        {
            manager = new MySQLManager();
            try
            {
                 return manager.CallStoredProcedure_Select("USP_Branch_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetAllBranch",
                    procedure_name = "USP_Branch_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetBranchesByUserId(string user_id)
        {
            manager = new MySQLManager();
            try
            {
                if (user_id == null)
                    manager.AddParameter(new MySqlParameter("p_user_id", DBNull.Value));
                else
                    manager.AddParameter(new MySqlParameter("p_user_id", user_id));

                return manager.CallStoredProcedure_Select("USP_Branch_ByUserId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetBranchesByUserId",
                    procedure_name = "USP_Branch_ByUserId",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public DataTable GetCounterCurrentStatus(int branch_id, int counter_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_counter_id", counter_id));

                return manager.CallStoredProcedure_Select("USP_Branch_CounterStatus");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetCounterCurrentStatus",
                    procedure_name = "USP_Branch_CounterStatus",
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
                manager.AddParameter(new MySqlParameter("p_branch_id", id));

                return manager.CallStoredProcedure_Select("USP_Branch_List_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "GetById",
                    procedure_name = "USP_Branch_List_ById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Insert(tblBranch branch)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(branch);

                long? branch_id = manager.CallStoredProcedure_Insert("USP_Branch_Insert");

                if (branch_id.HasValue) return (int)branch_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranch",
                    procedure_name = "USP_Branch_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        private void MapParameters(tblBranch branch)
        {
            manager.AddParameter(new MySqlParameter("p_branch_name", branch.branch_name));
            manager.AddParameter(new MySqlParameter("p_address", branch.address));
            manager.AddParameter(new MySqlParameter("p_contact_no", branch.contact_no));
            manager.AddParameter(new MySqlParameter("p_contact_person", branch.contact_person));
            manager.AddParameter(new MySqlParameter("p_display_next", branch.display_next));
            manager.AddParameter(new MySqlParameter("p_static_ip", branch.static_ip));
        }
        public void Update(tblBranch branch)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch.branch_id));
                MapParameters(branch);
                manager.CallStoredProcedure_Update("USP_Branch_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranch",
                    procedure_name = "USP_Branch_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", id));

                manager.CallStoredProcedure_Update("USP_Branch_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranch",
                    procedure_name = "USP_Branch_Delete",
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
