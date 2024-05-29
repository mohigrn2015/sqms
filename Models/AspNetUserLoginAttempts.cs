using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class AspNetUserLoginAttempts
    {
        [Key]
        public long attempt_id { get; set; }

        public string LoginProvider { get; set; }

        public int branch_id { get; set; }

        [Display(Name = "Branch Name")]
        public string branch_name { get; set; }

        [Display(Name = "User ID")]
        public string user_id { get; set; }

        [StringLength(256)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public int counter_id { get; set; }

        [Display(Name = "Counter No")]
        public string counter_no { get; set; }

        [StringLength(20)]
        [Display(Name = "IP Address")]
        public string ip_address { get; set; }

        [StringLength(100)]
        [Display(Name = "Machine Name")]
        public string machine_name { get; set; }

        public DateTime attempt_time { get; set; }

        [Display(Name = "Attempt Time")]
        public string attempt_time_formatted
        {
            get
            {
                return attempt_time.ToString("dd/MMM/yyyy hh:mm:ss tt");
            }
        }
        public int is_success { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; } //=> (is_success == 0 ? "Fail" : "Success");


    }
    public class AspNetUserLoginAttempts_reprt
    {
        public string branch_name { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public DateTime attempt_time { get; set; }
        public string counter_no { get; set; }
        public string ip_address { get; set; }
        public string machine_name { get; set; }        
        public string status { get; set; }
    }
}
