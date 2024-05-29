using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_LogoutDetail
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }

        [Display(Name = "Agent ID")]
        public string user_id { get; set; }

        [Display(Name = "Counter")]
        public string counter_no { get; set; }

        [Display(Name = "Date")]
        public string service_date_formated
        {
            get; set;
        } 
        public string Date { get; set; }
        public string login_time { get; set; }
        public DateTime login_timeV2 { get; set; }

        [Display(Name = "Login Time")]
        public string login_time_formated
        {
            get; set;
        }

        [Display(Name = "Logout Time")]
        public string logout_time_formated { get; set; }

        [Display(Name = "Duration")]
        public string duration { get; set; }

        [Display(Name = "Reason")]
        public string logout_reason { get; set; }
    }
    public class RM_LogoutDetail_Report
    {
        public string branch_name { get; set; }
        public string user_name { get; set; }
        public string counter_no { get; set; }
        public DateTime Date { get; set; }
        public DateTime login_time { get; set; }
        public string logout_time_formated { get; set; }
        public string duration { get; set; }
        public string logout_reason { get; set; }
    }
}
