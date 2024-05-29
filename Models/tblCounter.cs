using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblCounter
    {
        public tblCounter()
        {
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
            this.tblDailyBreaks = new HashSet<tblDailyBreak>();
        }
        public int counter_id { get; set; }
        public int branch_id { get; set; }
        public string counter_no { get; set; }
        public string location { get; set; }

        public int is_active { get; set; }
        //public virtual tblBranch tblBranch { get; set; }
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
        public virtual ICollection<tblDailyBreak> tblDailyBreaks { get; set; }
    }
}
