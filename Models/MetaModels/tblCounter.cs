using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(CounterMeta))]
    public partial class tblCounter
    {
    }

    public class CounterMeta
    {
        [Display(Name = "Counter No")]
        [Required]
        [StringLength(5)]
        public string counter_no { get; set; }

        [Display(Name = "Location")]
        [StringLength(250)]
        public string location { get; set; }

    }
}
