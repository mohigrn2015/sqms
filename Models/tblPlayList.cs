using SQMS.Models.MetaModels;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public partial class tblPlayList
    {
        public tblPlayList()
        {
            this.tblBranches = new HashSet<tblBranch?>();
        }

        public int playlist_id { get; set; } = 0;
        public string playlist_name { get; set; }
        public int is_global { get; set; } = 0;
        public virtual ICollection<tblBranch?> tblBranches { get; set; }

    }
}
