using SQMS.Utility;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(TokenQueueMeta))]
    public partial class tblTokenQueue
    {
        public Nullable<System.DateTime> CallTime { get; set; }
        public System.DateTime service_date { get; set; }
        public string token_prefix { get; set; }
        public int token_no { get; set; }
        public string waitingtime
        {
            get
            {
                if (CallTime.HasValue)

                    return CallTime.Value.Subtract(service_date).ToString();
                else return "";

            }
        }

        public TimeSpan waitingtimeToTimeStamp
        {
            get
            {
                if (CallTime.HasValue)

                    return CallTime.Value.Subtract(service_date);
                else return default(TimeSpan);

            }
        }

        public string token_no_formated
        {
            get
            {
                return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }
    }

    public class TokenQueueMeta
    {

        [Display(Name = "Service Type")]
        [Required]
        public int service_type_id { get; set; }

        [Display(Name = "Contact No")]
        public string contact_no { get; set; }

        [Display(Name = "Token No")]
        [Required]
        public int token_no { get; set; }

        [Display(Name = "Service Date")]
        [Required]
        public System.DateTime service_date { get; set; }

        [Display(Name = "Service Status")]
        [Required]
        public short service_status_id { get; set; }

        [Display(Name = "Counter No")]
        public Nullable<int> counter_id { get; set; }

        [Display(Name = "User")]
        public string user_id { get; set; }

        [Display(Name = "Cancel Time")]
        public Nullable<System.DateTime> cancel_time { get; set; }
        [Display(Name = "Call Time")]

        public Nullable<System.DateTime> CallTime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tblBranch tblBranch { get; set; }
        public virtual tblCounter tblCounter { get; set; }
        public virtual ICollection<tblServiceDetail> tblServiceDetails { get; set; }
        public virtual tblServiceStatu tblServiceStatu { get; set; }
        public virtual tblServiceType tblServiceType { get; set; }


    }
}
