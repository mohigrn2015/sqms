namespace SQMS.Models.RequestModel
{
    public class SendSMSRequestModel
    {
        public string mobile { get; set; } = string.Empty;
        public string tokenNo { get; set; }
        public string securityToken { get; set; } = string.Empty;
    }
}
