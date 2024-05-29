namespace SQMS.Models.RequestModel
{
    public class NewTokenReqModel
    {
        public string mobile { get; set; }
        public int service_type_id { get; set; }
        public string securityToken { get; set; }
    }
}