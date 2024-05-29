using MySql.Data.MySqlClient;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALServiceDetail
    {
        MySQLManager manager;

        public DataTable GetAllCurrentDate(int? branch_id, string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_BRANCH_ID", Value = branch_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USER_ID", Value = user_id });

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectCDate");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_ServiceDetail_SelectCDate",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }


        public DataTable GetAllServices(int? branch_id, string user_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_BRANCH_ID", Value = branch_id });
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_USER_ID", Value = user_id });

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectAll");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_ServiceDetail_SelectAll",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataTable GetByCustomerID(long customer_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter() { ParameterName = "P_CUSTOMER_ID", Value = customer_id });

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectByCId");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_ServiceDetail_SelectByCId",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public List<tblServiceSubType> GetNewToken(int branch_id, int counter_id, string userid, out long token_id, out string token_prefix, out int token_no, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address, out DateTime generate_time, out int is_break)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_BRANCH_ID", branch_id));
                manager.AddParameter(new MySqlParameter("P_COUNTER_ID", counter_id));
                manager.AddParameter(new MySqlParameter("P_USER_ID", userid));

                DataSet resultSets = manager.CallStoredProcedure_SelectDataSet("USP_SERVICEDETAIL_NEWCALL");
                DataTable firstResultSet = resultSets.Tables[0];
                DataTable secondResultSet = resultSets.Tables[1];

                token_id = 0;
                token_prefix = "";
                token_no = 0;
                contact_no = null;
                service_type = null;
                start_time = DateTime.Now;
                customer_name = null;
                is_break = 0;
                address = null;
                generate_time = DateTime.Now;

                if (firstResultSet.Rows.Count > 0)
                {
                    foreach (DataRow row in firstResultSet.Rows)
                    {
                        token_id = Convert.ToInt64((row["PO_TOKEN_ID"] == DBNull.Value ? null : row["PO_TOKEN_ID"].ToString()));
                        token_prefix = row["PO_TOKEN_PREFIX"] == DBNull.Value ? null : row["PO_TOKEN_PREFIX"].ToString();
                        token_no = Convert.ToInt32((row["PO_TOKEN_NO"] == DBNull.Value ? null : row["PO_TOKEN_NO"].ToString()));
                        contact_no = row["PO_CONTACT_NO"] == DBNull.Value ? null : row["PO_CONTACT_NO"].ToString();
                        service_type = row["PO_SERVICE_TYPE"] == DBNull.Value ? null : row["PO_SERVICE_TYPE"].ToString();
                        start_time = Convert.ToDateTime(row["PO_START_TIME"] == DBNull.Value ? null : row["PO_START_TIME"].ToString());
                        customer_name = row["PO_CUSTOMER_NAME"] == DBNull.Value ? null : row["PO_CUSTOMER_NAME"].ToString();
                        is_break = Convert.ToInt32((row["PO_IS_BREAK_ASSIGNED"] == DBNull.Value ? null : row["PO_IS_BREAK_ASSIGNED"].ToString()));
                        address = row["PO_ADDRESS"] == DBNull.Value ? null : row["PO_ADDRESS"].ToString();
                        generate_time = Convert.ToDateTime(row["PO_SERVICE_DATE"] == DBNull.Value ? null : row["PO_SERVICE_DATE"].ToString());
                    }
                }
                BLLServiceSubType serviceSubTypeManager = new BLLServiceSubType();
                List<tblServiceSubType> serviceSubTypeList = new List<tblServiceSubType>();
                serviceSubTypeList = serviceSubTypeManager.ObjectMappingListTBL(secondResultSet);
                
                return serviceSubTypeList;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_SERVICEDETAIL_NEWCALL",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public List<tblServiceSubType> CallManualToken(int branch_id, int counter_id, string userid, string token_no, out long token_id, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_BRANCH_ID", branch_id));
                manager.AddParameter(new MySqlParameter("P_COUNTER_ID", counter_id));
                manager.AddParameter(new MySqlParameter("P_USER_ID", userid));
                manager.AddParameter(new MySqlParameter("P_TOKEN_NO", token_no));

                token_id = 0;
                contact_no = null;
                service_type = null;
                start_time = DateTime.Now;
                customer_name = null;
                address = null;

                DataSet resultSets = manager.CallStoredProcedure_SelectDataSet("USP_SERVICEDETAIL_MANUALCALL");

                // Accessing individual result sets
                DataTable firstResultSet = resultSets.Tables[0];
                DataTable secondResultSet = resultSets.Tables[1];



                //DataTable dt = manager.CallStoredProcedure_Select("USP_SERVICEDETAIL_MANUALCALL");
                if (firstResultSet.Rows.Count > 0)
                {
                    foreach (DataRow row in firstResultSet.Rows)
                    {
                        token_id = Convert.ToInt64((row["PO_TOKEN_ID"] == DBNull.Value ? null : row["PO_TOKEN_ID"].ToString()));
                        contact_no = row["PO_CONTACT_NO"] == DBNull.Value ? null : row["PO_CONTACT_NO"].ToString();
                        service_type = row["PO_SERVICE_TYPE"] == DBNull.Value ? null : row["PO_SERVICE_TYPE"].ToString();
                        start_time = Convert.ToDateTime(row["PO_START_TIME"] == DBNull.Value ? null : row["PO_START_TIME"].ToString());
                        customer_name = row["PO_CUSTOMER_NAME"] == DBNull.Value ? null : row["PO_CUSTOMER_NAME"].ToString();
                        address = row["PO_ADDRESS"] == DBNull.Value ? null : row["PO_ADDRESS"].ToString();
                    }
                }
                else
                {
                    token_id = 0;
                    contact_no = null;
                    service_type = null;
                    start_time = DateTime.Now;
                    customer_name = null;
                    address = null;
                }
                BLLServiceSubType serviceSubTypeManager = new BLLServiceSubType();
                List<tblServiceSubType> serviceSubTypeList = serviceSubTypeManager.ObjectMappingListTBL(secondResultSet);

                return serviceSubTypeList;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_SERVICEDETAIL_MANUALCALL",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int CancelToken(long token_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_token_id", token_id));

                return (int)manager.CallStoredProcedure_Insert("USP_Token_Cancel");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_Token_Cancel",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Transfer(int branch_id, string counter_no, long token_id)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_branch_id", branch_id));
                manager.AddParameter(new MySqlParameter("p_counter_no", counter_no));
                manager.AddParameter(new MySqlParameter("p_token_id", token_id));

                return (int)manager.CallStoredProcedure_Insert("USP_SERVICEDETAIL_TRANSFER");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_SERVICEDETAIL_TRANSFER",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public int Insert(VMServiceDetails servicedetail)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(servicedetail);
                long? service_id = manager.CallStoredProcedure_Insert("USP_SERVICEDETAIL_INSERT");
                if (service_id.HasValue) return (int)service_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_SERVICEDETAIL_INSERT",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public int AddService(VMServiceDetails servicedetail)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(servicedetail);
                long? service_id = manager.CallStoredProcedure_Insert("USP_SERVICEDETAIL_ADDSERV_V8");
                if (service_id.HasValue) return (int)service_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALServiceDetail",
                    procedure_name = "USP_SERVICEDETAIL_ADDSERV_V8",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(VMServiceDetails servicedetail)
        {
            manager.AddParameter(new MySqlParameter("P_TOKEN_ID", servicedetail.token_id));
            manager.AddParameter(new MySqlParameter("P_CONTACT_NO", servicedetail.contact_no));
            manager.AddParameter(new MySqlParameter("P_IS_PRIMARY_CONTACT", servicedetail.is_primary_contact));
            manager.AddParameter(new MySqlParameter("P_START_TIME", servicedetail.start_time));
            manager.AddParameter(new MySqlParameter("P_SERVICE_SUB_TYPE_ID", servicedetail.service_sub_type_id));
            manager.AddParameter(new MySqlParameter("P_ISSUES", servicedetail.issues));
            manager.AddParameter(new MySqlParameter("P_SOLUTIONS", servicedetail.solutions));
            manager.AddParameter(new MySqlParameter("P_CUSTOMER_NAME", servicedetail.customer_name));
            manager.AddParameter(new MySqlParameter("P_ADDRESS", servicedetail.address));
            manager.AddParameter(new MySqlParameter("P_COUNTER_ID", servicedetail.counter_id));
            manager.AddParameter(new MySqlParameter("P_USER_ID", servicedetail.user_id));
            manager.AddParameter(new MySqlParameter("P_REFRESH_REASON", servicedetail.refresh_reason));
        }
    }
}
