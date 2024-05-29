using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblServiceType
    {
        public tblServiceType()
        {
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
            this.tblServiceSubTypes = new HashSet<tblServiceSubType>();
        }
        public int service_type_id { get; set; }
        public string service_type_name { get; set; }
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
        public virtual ICollection<tblServiceSubType> tblServiceSubTypes { get; set; }
    }
}
