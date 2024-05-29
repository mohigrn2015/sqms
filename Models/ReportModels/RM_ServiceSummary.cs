using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_ServiceSummary
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Token Served")]
        public int token_served { get; set; }

        [Display(Name = "%Total")]
        public decimal total_percentage { get; set; }

        [Display(Name = "Standard Time")]
        public string standard_time { get; set; }

        [Display(Name = "Actual Time")]
        public string actual_time { get; set; }

        [Display(Name = "Variance")]
        public string variance { get; set; }
    }

    public class RM_ServiceSummary_report
    {
        public string branch_name { get; set; }
        public string service_sub_type_name { get; set; }
        public int token_served { get; set; }
        public decimal total_percentage { get; set; }
        public string standard_time { get; set; }
        public string actual_time { get; set; }
        public string variance { get; set; }
    }
}
