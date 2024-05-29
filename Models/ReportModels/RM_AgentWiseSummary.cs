using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_AgentWiseSummary
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }

        [Display(Name = "Agent id")]
        public string user_id { get; set; }

        [Display(Name = "Handled Customers")]
        public string handled_customer { get; set; }

        [Display(Name = "Average Waiting Time")]
        public string average_waiting_time { get; set; }

        [Display(Name = "Average Service Time")]
        public string average_service_time { get; set; }

        [Display(Name = "Avg. TAT")]
        public string average_TAT { get; set; }
    }
    public class RM_AgentWiseSummary_report
    {
        public string branch_name { get; set; }
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string handled_customer { get; set; }
        public string average_waiting_time { get; set; }
        public string average_service_time { get; set; }
        public string average_TAT { get; set; }
    }
}
