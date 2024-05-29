namespace SQMS.Models.RequestModel
{
    public class Token_SMSRequestModel
    {
        public string msisdn { get; set; }
        public int is_bn { get; set; }
        public string token_no { get; set; }
        public string token_message { get; set; }
    }
}
