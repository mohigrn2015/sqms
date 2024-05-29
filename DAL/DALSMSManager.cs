using MySql.Data.MySqlClient;
using SQMS.Utility;

namespace SQMS.DAL
{
    public class DALSMSManager
    {
        MySQLManager manager;
        public void SendSMS(string msisdn, string message)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_MSISDN", msisdn));
                manager.AddParameter(new MySqlParameter("P_MESSAGE", message));
                manager.CallStoredProcedure("USP_SENDSMS");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALSMSManager",
                    procedure_name = "USP_SENDSMS",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }

        public void SendSMSBn(string msisdn, string messageBn, string tokenBn)
        {
            manager = new MySQLManager();
            try
            {
                manager.AddParameter(new MySqlParameter("P_MSISDN", msisdn));
                MySqlParameter P_FULL_MESSAGE_BN = new MySqlParameter();
                P_FULL_MESSAGE_BN.ParameterName = "P_FULL_MESSAGE_BN";
                P_FULL_MESSAGE_BN.Value = messageBn;
                manager.AddParameter(P_FULL_MESSAGE_BN);

                MySqlParameter P_TOKEN_NO_BN = new MySqlParameter();
                P_TOKEN_NO_BN.ParameterName = "P_TOKEN_NO_BN";
                P_TOKEN_NO_BN.Value = tokenBn;
                manager.AddParameter(P_TOKEN_NO_BN);

                manager.CallStoredProcedure("USP_SENDSMS_BN");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALSMSManager",
                    procedure_name = "USP_SENDSMS_BN",
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
