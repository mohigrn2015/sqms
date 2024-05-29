namespace SQMS.Models
{
    public class ChangePassReqModel
    {
        public string userId { get; set; }
        public string currPass { get; set; }
        public string newPass { get; set; }
        public string sequrityStamp { get; set; }
        public string userName { get; set; }
    }
}
