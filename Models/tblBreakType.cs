using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblBreakType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBreakType()
        {
            this.tblDailyBreaks = new HashSet<tblDailyBreak>();
        }

        public int break_type_id { get; set; }
        public string break_type_short_name { get; set; }
        public string break_type_name { get; set; }
        public string break_type_with_duration { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public int duration { get; set; }
        public virtual ICollection<tblDailyBreak> tblDailyBreaks { get; set; }
    }
}
