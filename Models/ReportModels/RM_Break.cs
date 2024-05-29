using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_Break
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Date")]
        public string create_time { get; set; }

        [Display(Name = "User")]
        public string username { get; set; }

        [Display(Name = "User ID")]
        public string userid { get; set; }

        [Display(Name = "Counter")]
        public string counter_no { get; set; }

        [Display(Name = "Reason For Break")]
        public string break_type_name { get; set; }

        [Display(Name = "Start Time")]
        public string start_time { get; set; }

        [Display(Name = "End Time")]
        public string end_time { get; set; }

        [Display(Name = "Duration")]
        public string duration { get; set; }

    }
    public class RM_Break_report
    {
        public string branch_name { get; set; }
        public string create_time { get; set; }
        public string username { get; set; }
        public string counter_no { get; set; }
        public string break_type_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string duration { get; set; }
    }
}
