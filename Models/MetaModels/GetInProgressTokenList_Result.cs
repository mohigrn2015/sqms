using System.ComponentModel.DataAnnotations;
using SQMS.Models;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(GetInProgressTokenList_ResultrMeta))]
    public partial class GetInProgressTokenList_Result
    {
        public string token_no { get; set; }
        public string token_no_with_pad
        {
            get
            {
                return token_no.PadLeft(3);
            }
        }
    }

    public class GetInProgressTokenList_ResultrMeta
    {
        [Display(Name = "Counter No")]
        [Required]
        [StringLength(5)]
        public string counter_no { get; set; }



    }
}
