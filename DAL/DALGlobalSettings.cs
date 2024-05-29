using MySql.Data.MySqlClient;
using SQMS.Models;
using SQMS.Utility;
using System.Data;

namespace SQMS.DAL
{
    public class DALGlobalSettings
    {
        MySQLManager manager = new MySQLManager();

        public DataTable Get()
        {
            try
            {
                return manager.CallStoredProcedure_Select("USP_GLOBALSETTINGS_GET");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALGlobalSettings",
                    procedure_name = "USP_GLOBALSETTINGS_GET",
                    error_source = ex.Source,
                    error_code = ex.HResult,
                    error_description = ex.Message
                });
                TextLogger textLogger = new TextLogger();
                textLogger.LogWrite(text);
                throw new Exception(ex.Message.ToString());
            }
        }
        public void Update(tblGlobalSettings tblGlobalSettings)
        {
            try
            {
                manager.AddParameter(new MySqlParameter("p_tat_visibility_time", tblGlobalSettings.tat_visibility_time));
                manager.AddParameter(new MySqlParameter("p_notification_visibility_days", tblGlobalSettings.notification_visibility_days));
                manager.AddParameter(new MySqlParameter("p_is_msg_bn", (tblGlobalSettings.is_msg_bn == true ? 1 : 0)));
                manager.AddParameter(new MySqlParameter("p_default_token_prefix", tblGlobalSettings.default_token_prefix));
                manager.AddParameter(new MySqlParameter("p_padding_left", tblGlobalSettings.padding_left));
                manager.AddParameter(new MySqlParameter("p_msg_text", tblGlobalSettings.msg_text));
                manager.AddParameter(new MySqlParameter("p_display_footer_ad", tblGlobalSettings.display_footer_ad));
                manager.AddParameter(new MySqlParameter("p_report_csv_separator", tblGlobalSettings.report_csv_separator));
                
                manager.CallStoredProcedure_Update("USP_GLOBALSETTING_UPDATE");
            }
            catch (Exception ex)
            {
                string? text = Convert.ToString(new
                {
                    request_time = DateTime.Now,
                    method_name = "DALGlobalSettings",
                    procedure_name = "USP_GLOBALSETTING_UPDATE",
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
