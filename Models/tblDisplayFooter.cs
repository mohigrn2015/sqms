using SQMS.Models.MetaModels;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public partial class tblDisplayFooter
    {
        public tblDisplayFooter()
        {
            this.tblBranches = new HashSet<tblBranch>();
        }

        [Key]
        public int display_footer_id { get; set; }
        public string content_en { get; set; }
        public string content_bn { get; set; }
        public int is_global { get; set; }
        public virtual ICollection<tblBranch> tblBranches { get; set; }

    }
}
