using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.ViewModels
{
    public class VMBranchLogin
    {
        [Display(Name = "Branch ID")]
        public int branch_id { get; set; }
        [Display(Name = "User")]
        public string user_id { get; set; }
        public int counter_id { get; set; }
        public int user_branch_id { get; set; }
        public string branch_name { get; set; }
        public string Hometown { get; set; }


        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string counter_no { get; set; }
        public string MyProperty8 { get; set; }
        public string MyProperty9 { get; set; }
        public int is_active { get; set; }
        public int is_active_directory_user { get; set; }
    }
}
