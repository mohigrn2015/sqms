namespace SQMS.Models
{
    public class tblAgentTransferLog
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public DateTime transfer_date { get; set; }
        public int from_branch_id { get; set; }
        public int to_branch_id { get; set; }
        public string transfer_by { get; set; }
    }
}
