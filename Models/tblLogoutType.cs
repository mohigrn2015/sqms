using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public partial class tblLogoutType
    {
        public tblLogoutType()
        {
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
        }

        public int logout_type_id { get; set; }

        [Display(Name = "Logout Type Name")]
        [Required]
        public string logout_type_name { get; set; }

        [Display(Name = "Free Text?")]
        public int has_free_text { get; set; }



        [Display(Name = "Is Active?")]
        public int is_active { get; set; }

        [Required]
        public bool bool_has_free_text
        {
            get
            {
                return (has_free_text == 1 ? true : false);
            }
            set
            {
                has_free_text = (value == true ? 1 : 0);
            }
        }
        [Required]
        public bool bool_is_active
        {
            get
            {
                return (is_active == 1 ? true : false);
            }
            set
            {
                is_active = (value == true ? 1 : 0);
            }
        }

        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
    }
}
