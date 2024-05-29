using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_SingleVSMultipleVisitedCustomer
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Total Served Token")]
        public string total_served_token { get; set; }

        [Display(Name = "Single Visited Customers")]
        public string single_visit_customer { get; set; }

        [Display(Name = "Multiple Visited Customers")]
        public string multiple_visit_customer { get; set; }
    }
    public class RM_SingleVSMultipleVisitedCustomer_report
    {
        public string branch_name { get; set; }
        public string service_sub_type_name { get; set; }
        public string total_served_token { get; set; }
        public string single_visit_customer { get; set; }
        public string multiple_visit_customer { get; set; }
    }
}
