using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblServiceSubType
    {
        public tblServiceSubType()
        {
            this.tblServiceDetails = new HashSet<tblServiceDetail>();
        }
        public int service_sub_type_id { get; set; }
        public int service_type_id { get; set; }
        public string service_sub_type_name { get; set; }
        public int max_duration { get; set; }
        public int counter_id { get; set; }
        public int is_active { get; set; }
        public virtual ICollection<tblServiceDetail> tblServiceDetails { get; set; }
        public virtual tblServiceType? tblServiceType { get; set; }
        public int tat_warning_time { get; set; }
         
    }
}
