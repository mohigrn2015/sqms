using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class RM_CentreWiseSummary
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Handled Customers")]
        public string handled_customer { get; set; }
        public string total_servecd { get; set; }

        [Display(Name = "Average Waiting Time")]
        public string average_waiting_time { get; set; }

        [Display(Name = "Total STD Time")]
        public string total_std_time { get; set; }

        [Display(Name = "Actual Serving Time")]
        public string actual_serving_time { get; set; }

        [Display(Name = "Variance")]
        public string variance { get; set; }

        [Display(Name = "Average Service Time")]
        public string average_service_time { get; set; }
    }

    public class RM_CentreWiseSummary_report
    {
        public string branch_name { get; set; }
        public string handled_customer { get; set; }
        public string total_servecd { get; set; }
        public string average_waiting_time { get; set; }
        public string total_std_time { get; set; }
        public string actual_serving_time { get; set; }
        public string variance { get; set; }
        public string average_service_time { get; set; }
    }
}
