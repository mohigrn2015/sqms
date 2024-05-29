using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class tblGlobalSettings
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Warning Pop-Up (sec)")]
        public int tat_visibility_time { get; set; }

        [Display(Name = "Notification Visibility (days)")]
        public int notification_visibility_days { get; set; }

        [Display(Name = "Is Message Bangla")]
        public bool is_msg_bn { get; set; }

        [Display(Name = "Default Token Prefix")]
        public string default_token_prefix { get; set; } = string.Empty;

        [Display(Name = "Padding Left")]
        public int padding_left { get; set; }

        [Display(Name = "Message Text")]
        public string msg_text { get; set; } = string.Empty;

        [Display(Name = "Message Text Bn")]
        public string msg_text_Bn { get; set; } = string.Empty;

        [Display(Name = "Display Footer Ad")]
        public string display_footer_ad { get; set; } = string.Empty;

        [Display(Name = "Report CSV Separator")]
        public string report_csv_separator { get; set; } = string.Empty;
    }

}
