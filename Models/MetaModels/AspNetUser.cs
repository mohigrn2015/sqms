using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(AspNetUserMeta))]
    public partial class AspNetUser
    {
    }

    public class AspNetUserMeta
    {
        [Display(Name = "User")]
        [Required]
        public string Hometown { get; set; }
    }
}
