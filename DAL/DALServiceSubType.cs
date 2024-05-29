using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALServiceSubType
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_ServiceSubType_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_SelectAll",
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
                manager.AddParameter(new MySqlParameter("p_servicesub_type_id", id));

                return manager.CallStoredProcedure_Select("USP_ServiceSubType_List_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_List_ById",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetByTypeId(int service_type_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_service_type_id", service_type_id));
                
                return manager.CallStoredProcedure_Select("USP_ServiceSubType_List_ByTId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_List_ByTId",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Call only for New Service Type Insert
        /// Return service_type_id
        /// </summary>
        /// <param name="serviceType">Service Type Object</param>
        /// <returns>Return service_type_id</returns>
        public int Insert(tblServiceSubType servicesubType)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(servicesubType);
                long? service_sub_type_id = manager.CallStoredProcedure_Insert("USP_ServiceSubType_Insert_V8");
                if (service_sub_type_id.HasValue) return (int)service_sub_type_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_Insert_V8",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Update(tblServiceSubType servicesubType)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(servicesubType);

                manager.CallStoredProcedure_Update("USP_SERVICESUBTYPE_UPDATE_V8");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_SERVICESUBTYPE_UPDATE_V8",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void SetStatus(int service_sub_type_id, int is_active)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_servicesub_type_id", service_sub_type_id));
                manager.AddParameter(new MySqlParameter("p_is_active", is_active));

                manager.CallStoredProcedure_Update("USP_ServiceSubType_SetStatus");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_SetStatus",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(tblServiceSubType servicesubType)
        {
            manager.AddParameter(new MySqlParameter("p_servicesub_type_name", servicesubType.service_sub_type_name));
            manager.AddParameter(new MySqlParameter("p_service_type_id", servicesubType.service_type_id));
            manager.AddParameter(new MySqlParameter("p_max_duration", servicesubType.max_duration));
            manager.AddParameter(new MySqlParameter("p_servicesub_type_id", servicesubType.service_sub_type_id));
            manager.AddParameter(new MySqlParameter("p_tat_warning_time", servicesubType.tat_warning_time));
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_servicesub_type_id", id));

                manager.CallStoredProcedure_Update("USP_ServiceSubType_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_ServiceSubType_Delete",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void UpdateTatBulk(string sub_type_id, int time)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_service_sub_type_id", sub_type_id));
                manager.AddParameter(new MySqlParameter("p_tat_time", time));

                manager.CallStoredProcedure_Update("USP_SERVICESUBTYPE_TAT_BULK_U");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceSubType",
                    procedure_name = "USP_SERVICESUBTYPE_TAT_BULK_U",
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
