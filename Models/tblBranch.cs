using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblBranch
    {
        public tblBranch()
        {
            this.tblBranchUsers = new HashSet<tblBranchUser>();
            this.tblCounters = new HashSet<tblCounter>();
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
        }

        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string contact_person { get; set; }
        public string contact_no { get; set; }
        public int display_next { get; set; }
        public string static_ip { get; set; }
        public virtual ICollection<tblBranchUser> tblBranchUsers { get; set; }
        public virtual ICollection<tblCounter> tblCounters { get; set; }
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
    }
}
