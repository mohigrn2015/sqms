using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALMSISDNAndDeviceInfo
    {
        MySQLManager manager;

        public int InsertHandsetLog(VMHandsetLog handsetLog)
        {
            manager = new MySQLManager();
            try
            {
                MapParameters(handsetLog);
                long? log_id = manager.CallStoredProcedure_Insert("USP_HANDSET_LOG_INSERT");
                if (log_id.HasValue) return (int)log_id.Value;
                else return 0;
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALMSISDNAndDeviceInfo",
                    procedure_name = "USP_HANDSET_LOG_INSERT",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        private void MapParameters(VMHandsetLog handsetLog)
        {
            manager.AddParameter(new MySqlParameter("P_MSISDN", handsetLog.msisdn));
            manager.AddParameter(new MySqlParameter("P_REQUEST_TIME", handsetLog.request_date));

            MySqlParameter paramRequestData = new MySqlParameter();
            paramRequestData.ParameterName = "P_REQUEST_DATA";
            paramRequestData.Value = handsetLog.request_data;
            manager.AddParameter(paramRequestData);

            manager.AddParameter(new MySqlParameter("P_RESPONSE_TIME", handsetLog.response_date));

            MySqlParameter paramResponseData = new MySqlParameter();
            paramResponseData.ParameterName = "P_RESPONSE_DATA";
            paramResponseData.Value = handsetLog.request_data;
            manager.AddParameter(paramResponseData);

            manager.AddParameter(new MySqlParameter("P_API_STATUS", handsetLog.api_status));
            manager.AddParameter(new MySqlParameter("P_MESSAGE", handsetLog.message));
        }

        public DataTable GetDeviceLoanInfoByMSISDN(string msisdn)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("p_msisdn", msisdn));

                return manager.CallStoredProcedure_Select("USP_DFP_BY_MSISDN");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALMSISDNAndDeviceInfo",
                    procedure_name = "USP_DFP_BY_MSISDN",
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
