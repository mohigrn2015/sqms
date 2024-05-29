using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_TokenExceeding
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "User Name")]
        public string user_name { get; set; }

        [Display(Name = "User ID")]
        public string user_id { get; set; }

        [Display(Name = "Total Served Tokens")]
        public int total_served_token { get; set; }

        [Display(Name = "Total Exceeding Tokens")]
        public int total_exceedig_token { get; set; }
    }
    public class RM_TokenExceeding_report
    {
        public string branch_name { get; set; }
        public string user_name { get; set; }  
        public int total_served_token { get; set; }
        public int total_exceedig_token { get; set; }
    }
}
