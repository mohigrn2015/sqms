using MySql.Data.MySqlClient;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALBranchPlayList
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_Branch_PL_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchPlayList",
                    procedure_name = "USP_Branch_PL_SelectAll",
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
                manager.AddParameter(new MySqlParameter("p_branch_playlist_id", id));
                
                return manager.CallStoredProcedure_Select("USP_Branch_PL_SelectById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchPlayList",
                    procedure_name = "USP_Branch_PL_SelectById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Insert(VMBranchPlayList branchPlayList)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(branchPlayList);
                long? playlist_id = manager.CallStoredProcedure_Insert("USP_Branch_PL_Insert");
                if (playlist_id.HasValue) return (int)playlist_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchPlayList",
                    procedure_name = "USP_Branch_PL_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void Update(VMBranchPlayList branchPlayList)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_playlist_id", branchPlayList.branch_playlist_id));
                MapParameters(branchPlayList);
                manager.CallStoredProcedure_Update("USP_Branch_PL_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchPlayList",
                    procedure_name = "USP_Branch_PL_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(VMBranchPlayList displayFooter)
        {
            manager.AddParameter(new MySqlParameter("p_branch_id", displayFooter.branch_id));
            manager.AddParameter(new MySqlParameter("p_playlist_id", displayFooter.playlist_id)); ;
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_playlist_id", id));

                manager.CallStoredProcedure_Update("USP_Branch_PL_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALBranchPlayList",
                    procedure_name = "USP_Branch_PL_Delete",
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
