namespace SQMS.Models.ResponseModel
{
    public class OuterLoginResponseModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int branch_id { get; set; }
        public string securityToken { get; set; }
        public List<ServiceListModel> serviceList { get; set; }
        public string server_url { get; set; }
        public int appRequestTimeOut { get; set; }
    } 
    public class ServiceListModel
    {
        public int service_type_id { get; set; }
        public string service_type_name { get; set; }
    }

    public class InvalidResponse 
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
