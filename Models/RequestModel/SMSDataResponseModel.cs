namespace SQMS.Models.RequestModel
{
    public class SMSDataResponseModel
    {
        public string msisdn { get; set; }
        public int is_bn { get; set; }
        public string token_no { get; set; }
        public string message { get; set; }
        public string replay_address { get; set; }
        public string application { get; set; }
        public string status { get; set; }
        public int is_token_sms { get; set; }
    }
}
