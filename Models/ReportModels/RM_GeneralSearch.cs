using SQMS.Utility;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_GeneralSearch
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Date")]
        public string Service_date { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }
        [Display(Name = "Agent ID")]
        public string user_id { get; set; }
        public string token_no { get; set; }
        public string token_prefix { get; set; }

        [Display(Name = "Token No")]
        public string token_no_formated
        {
            get
            {
                if (token_no == ApplicationSetting.DisplayWhenEmptyToken)
                    return token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
                else
                    return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }

        [Display(Name = "Mobile No")]
        public string mobile_no { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Issue Time")]
        public string issue_time { get; set; }

        [Display(Name = "Start Time")]
        public string start_time { get; set; }

        [Display(Name = "End Time")]
        public string end_time { get; set; }

        [Display(Name = "Waiting Time")]
        public string wating_time { get; set; }

        [Display(Name = "Std. Time")]
        public string std_time { get; set; }

        [Display(Name = "Actual Time")]
        public string actual_time { get; set; }

        [Display(Name = "Variance")]
        public string variance { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Issues")]
        public string issues { get; set; }

        [Display(Name = "Solutions")]
        public string solutions { get; set; }

        [Display(Name = "Refresh Reason")]
        public string refresh_reason { get; set; }

    }

    public class RM_GeneralSearch_report
    {
        public string branch_name { get; set; }
        public string Service_date { get; set; }
        public string user_name { get; set; }
        public string token_no { get; set; }
        public string mobile_no { get; set; }
        public string service_sub_type_name { get; set; }
        public string issue_time { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string wating_time { get; set; }
        public string std_time { get; set; }
        public string actual_time { get; set; }
        public string variance { get; set; }
        public string remarks { get; set; }
    }
}
