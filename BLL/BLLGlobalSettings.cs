using SQMS.DAL;
using SQMS.Models;
using System.Data;

namespace SQMS.BLL
{
    public class BLLGlobalSettings
    {
        internal tblGlobalSettings ObjectMapping(DataTable dt)
        {
            tblGlobalSettings globalSettings = new tblGlobalSettings();
            foreach (DataRow row in dt.Rows)
            {
                globalSettings.tat_visibility_time = Convert.ToInt32(row["tat_visibility_time"] == DBNull.Value ? 0 : row["tat_visibility_time"]);
                globalSettings.notification_visibility_days = Convert.ToInt32(row["notification_visibility_days"] == DBNull.Value ? 0 : row["notification_visibility_days"]);
                globalSettings.is_msg_bn = (row["is_msg_bn"].ToString() == "1" ? true : false);
                globalSettings.default_token_prefix = row["default_token_prefix"] == DBNull.Value ? null : row["default_token_prefix"].ToString();
                globalSettings.padding_left = Convert.ToInt32(row["padding_left"] == DBNull.Value ? 0 : row["padding_left"]);
                globalSettings.msg_text = row["msg_text"] == DBNull.Value ? null : row["msg_text"].ToString();
                globalSettings.msg_text_Bn = row["MSG_TEXT_BN"] == DBNull.Value ? null : row["MSG_TEXT_BN"].ToString();
                globalSettings.display_footer_ad = row["display_footer_ad"] == DBNull.Value ? null : row["display_footer_ad"].ToString();
                globalSettings.report_csv_separator = row["report_csv_separator"] == DBNull.Value ? null : row["report_csv_separator"].ToString();
            }
            return globalSettings;
        }

        public tblGlobalSettings Get()
        {
            DALGlobalSettings dal = new DALGlobalSettings();
            DataTable dt = dal.Get();
            return ObjectMapping(dt);
        }
        public void Edit(tblGlobalSettings tblGlobalSettings)
        {
            DALGlobalSettings dal = new DALGlobalSettings();
            dal.Update(tblGlobalSettings);
        }
    }
}
