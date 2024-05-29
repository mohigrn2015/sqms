using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_TopNService
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_name { get; set; }

        [Display(Name = "Total Servic")]
        public int total_service { get; set; }
    }
    public class RM_TopNService_report
    {
        public string branch_name { get; set; }
        public string service_name { get; set; }
        public int total_service { get; set; }

    }
}
