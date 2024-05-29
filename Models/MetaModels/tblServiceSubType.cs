using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(ServiceSubTypeMeta))]
    public partial class tblServiceSubType
    {
        public int tat_warning_time { get; set; }
    }

    public class ServiceSubTypeMeta
    {
        [Display(Name = "Service Type")]
        [Required]
        public int service_type_id { get; set; }

        [Display(Name = "Service Name")]
        [Required]
        [StringLength(100)]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Duration (Minuites)")]
        [Required]
        public int max_duration { get; set; }

        [Display(Name = "Warning Time")]
        [Required]
        public int tat_warning_time { get; set; }

    }
}
