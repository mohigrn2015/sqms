using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblServiceStatu
    {
        public tblServiceStatu()
        {
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
        }
        public short service_status_id { get; set; }
        public string service_status { get; set; }
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
    }
}
