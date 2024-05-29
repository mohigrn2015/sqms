using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ReportModels
{
    public class RM_LocalCustomer
    {
        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "Date")]
        public string service_datetime { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Service Delivery Time")]
        public string end_time { get; set; }

        [Display(Name = "Icon MSISDN")]
        public string contact_no { get; set; }

        [Display(Name = "Icon Name")]
        public string customer_name { get; set; }

        [Display(Name = "IM MSISDN")]
        public string im_msisdn { get; set; }

        [Display(Name = "IM Name")]
        public string im_name { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Further follow-up needed")]
        public string further_followUp_needed { get; set; }

        [Display(Name = "Follow-up date")]
        public string FollowUp_date { get; set; }         
    }

    public class RM_LocalCustomer_report
    {
        public string branch_name { get; set; }
        public string service_datetime { get; set; }
        public string service_sub_type_name { get; set; }
        public string end_time { get; set; }
        public string contact_no { get; set; }
        public string customer_name { get; set; }
        public string im_msisdn { get; set; }
        public string im_name { get; set; }
        public string remarks { get; set; }
        public string further_followUp_needed { get; set; }
        public string FollowUp_date { get; set; }
    }
}
