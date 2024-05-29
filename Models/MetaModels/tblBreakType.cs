using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(BreakTypeMeta))]
    public partial class tblBreakType
    {
        [StringLength(150)]
        public string break_type_name { get; set; }
        public int duration { get; set; }
        public string break_name_with_duration
        {
            get
            {
                return break_type_name + " (" + duration.ToString() + " minutes)";
            }
        }
    }

    public class BreakTypeMeta
    {
        [Display(Name = "Short Name")]
        [Required]
        [StringLength(5)]
        public string break_type_short_name { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        [StringLength(150)]
        public string break_type_name { get; set; }

        [Display(Name = "Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public Nullable<System.DateTime> start_time { get; set; }

        [Display(Name = "End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public Nullable<System.DateTime> end_time { get; set; }

        [Display(Name = "Duration (Minuites)")]
        [Required]
        public int duration { get; set; }

    }
}
