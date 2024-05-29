namespace SQMS.Models
{
    public class tblUntagLog
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public DateTime un_tag_date { get; set; }
        public int branch_id { get; set; }
        public string transfer_by { get; set; }
    }
}
