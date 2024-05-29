using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(DailyBreakMeta))]
    public partial class tblDailyBreak
    {
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public DateTime? start_time { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public DateTime? end_time { get; set; }

        public string duration
        {
            get
            {
                if (end_time.HasValue && start_time.HasValue)
                    return end_time.Value.Subtract(start_time.Value).ToString();
                else return "";

            }
        }
    }

    public class DailyBreakMeta
    {
        [Display(Name = "Break Type")]
        [Required]
        public int break_type_id { get; set; }

        [Display(Name = "User")]
        [Required]
        public string user_id { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public Nullable<System.DateTime> start_time { get; set; }

        [Display(Name = "End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm:ss tt}", NullDisplayText = "00:00")]
        public Nullable<System.DateTime> end_time { get; set; }
        //public Long diffTicks = (end_time - start_time).Ticks;
        [Display(Name = "Remarks")]
        [StringLength(250)]
        public string remarks { get; set; }
    }
}
