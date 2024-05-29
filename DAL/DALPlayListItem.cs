using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALPlayListItem
    {
        MySQLManager manager;
        public DataTable GetAll(int playlist_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_playlist_id", playlist_id));

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_SelectAll",
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
                manager.AddParameter(new MySqlParameter("p_playlistitem_id", id));

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_SelectById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetByFileName(string file_name)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_file_name", file_name));

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectByFileName");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_SelectByFileName",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetByBranchId(int branch_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));

                return manager.CallStoredProcedure_Select("USP_PL_SelectByBranchId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_SelectByBranchId",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public int Insert(tblPlayListItem playlistItem)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(playlistItem);
                long? playlistitem_id = manager.CallStoredProcedure_Insert("USP_PL_Item_Insert");
                if (playlistitem_id.HasValue) return (int)playlistitem_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Update(tblPlayListItem playlistItem)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_playlistitem_id", playlistItem.playlistitem_id));
                MapParameters(playlistItem);
                manager.CallStoredProcedure_Update("USP_PL_Item_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(tblPlayListItem playlistItem)
        {
            manager.AddParameter(new MySqlParameter("p_playlist_id", playlistItem.playlist_id));
            manager.AddParameter(new MySqlParameter("p_item_url", playlistItem.item_url));
            manager.AddParameter(new MySqlParameter("p_file_type", playlistItem.file_type));
            manager.AddParameter(new MySqlParameter("p_file_name", playlistItem.file_name));
            manager.AddParameter(new MySqlParameter("p_duration_in_second", playlistItem.duration_in_second));
            manager.AddParameter(new MySqlParameter("p_sort_order", playlistItem.sort_order));
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_playlistitem_id", id));

                manager.CallStoredProcedure_Update("USP_PL_Item_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALPlayListItem",
                    procedure_name = "USP_PL_Item_Delete",
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
