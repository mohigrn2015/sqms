using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblBranchUser
    {
        public int user_branch_id { get; set; }
        public string user_id { get; set; }
        public int branch_id { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tblBranch tblBranch { get; set; }
        public string transfer_by { get; set; }
    }
}
