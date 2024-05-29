using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALCustomerType
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();   
            try
            {
                return manager.CallStoredProcedure_Select("USP_CustomerType_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCustomerType",
                    procedure_name = "USP_CustomerType_SelectAll",
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
                manager.AddParameter(new MySqlParameter("p_Customer_type_id", id));

                return manager.CallStoredProcedure_Select("USP_CustomerType_List_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCustomerType",
                    procedure_name = "USP_CustomerType_List_ById",
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
        /// Call only for New Customer Type Insert
        /// Return Customer_type_id
        /// </summary>
        /// <param name="CustomerType">Customer Type Object</param>
        /// <returns>Return Customer_type_id</returns>
        public int Insert(tblCustomerType CustomerType)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(CustomerType);
                long? Customer_type_id = manager.CallStoredProcedure_Insert("USP_CustomerType_Insert");
                if (Customer_type_id.HasValue) return (int)Customer_type_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCustomerType",
                    procedure_name = "USP_CustomerType_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Update(tblCustomerType CustomerType)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_Customer_type_id", CustomerType.customer_type_id));
                MapParameters(CustomerType);
                manager.CallStoredProcedure_Update("USP_CustomerType_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCustomerType",
                    procedure_name = "USP_CustomerType_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(tblCustomerType CustomerType)
        {
            manager.AddParameter(new MySqlParameter("p_Customer_type_name", CustomerType.customer_type_name));
            manager.AddParameter(new MySqlParameter("p_priority", CustomerType.priority));
            manager.AddParameter(new MySqlParameter("p_token_prefix", CustomerType.token_prefix));
            manager.AddParameter(new MySqlParameter("p_is_default", CustomerType.is_default));
        }
        public void Delete(int id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_Customer_type_id", id));

                manager.CallStoredProcedure_Update("USP_CustomerType_Delete");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCustomerType",
                    procedure_name = "USP_CustomerType_Delete",
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
