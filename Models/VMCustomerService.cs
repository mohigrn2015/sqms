using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public partial class VMCustomerService
    {
        [Required]
        [Display(Name = "Mobile No")]
        public string contact_no { get; set; }
        [Required]
        [Display(Name = "Customer Name")]
        public string customer_name { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Problem")]
        public string issues { get; set; }

        [Display(Name = "Solution")]
        public string solutions { get; set; }
    }
}
