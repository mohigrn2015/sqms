using MySql.Data.MySqlClient;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALCounterCustomerTypes
    {
        MySQLManager manager;
        public DataTable GetAll()
        {
            manager = new MySQLManager();
            try
            {
                return manager.CallStoredProcedure_Select("USP_CounCustType_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCounterCustomerTypes",
                    procedure_name = "USP_CounCustType_SelectAll",
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
                manager.AddParameter(new MySqlParameter("p_counter_customer_type_id", id));

                return manager.CallStoredProcedure_Select("USP_CounCustType_ById");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCounterCustomerTypes",
                    procedure_name = "USP_CounCustType_ById",
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
        public int Insert(VMCounterCustomerType counterCustType)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(counterCustType);
                long? counter_id = manager.CallStoredProcedure_Insert("USP_CounCustType_Insert");
                if (counter_id.HasValue) return (int)counter_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCounterCustomerTypes",
                    procedure_name = "USP_CounCustType_Insert",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Update(VMCounterCustomerType counterCustType)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_counter_id", counterCustType.counter_id));
                manager.AddParameter(new MySqlParameter("p_customer_type_id", counterCustType.customer_type_id));
                manager.AddParameter(new MySqlParameter("p_counter_customer_type_id", counterCustType.counter_customer_type_id));

                manager.CallStoredProcedure_Update("USP_CounCustType_Update");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCounterCustomerTypes",
                    procedure_name = "USP_CounCustType_Update",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public void ActiveOrDeactive(VMCounterCustomerType counterCustType)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_is_active", counterCustType.is_active));
                manager.AddParameter(new MySqlParameter("p_counter_customer_type_id", counterCustType.counter_customer_type_id));

                manager.CallStoredProcedure_Update("USP_CounCustType_ActiDeact");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALCounterCustomerTypes",
                    procedure_name = "USP_CounCustType_ActiDeact",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(VMCounterCustomerType counterCustType)
        {
            manager.AddParameter(new MySqlParameter("p_branch_id", counterCustType.branch_id));
            manager.AddParameter(new MySqlParameter("p_counter_id", counterCustType.counter_id));
            manager.AddParameter(new MySqlParameter("p_customer_type_id", counterCustType.customer_type_id));
        }        
    }
}
