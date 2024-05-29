using SQMS.Models.MetaModels;
using SQMS.Utility;

namespace SQMS.Models
{
    public partial class tblTokenQueue
    {
        public tblTokenQueue()
        {
            this.tblServiceDetails = new HashSet<tblServiceDetail>();
        }
        public long token_id { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public int service_type_id { get; set; }
        public string service_type_name { get; set; }
        public string contact_no { get; set; }
        public string token_prefix { get; set; }
        public int token_no { get; set; }
        public System.DateTime service_date { get; set; }
        public short service_status_id { get; set; }
        public string service_status { get; set; }
        public Nullable<int> counter_id { get; set; }
        public string counter_no { get; set; }
        public string user_id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public Nullable<System.DateTime> cancel_time { get; set; }
        public Nullable<System.DateTime> CallTime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tblBranch tblBranch { get; set; }
        public virtual tblCounter tblCounter { get; set; }
        public virtual ICollection<tblServiceDetail> tblServiceDetails { get; set; }
        public virtual tblServiceStatu tblServiceStatu { get; set; }
        public virtual tblServiceType tblServiceType { get; set; }
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
}
