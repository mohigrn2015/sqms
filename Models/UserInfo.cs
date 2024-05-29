namespace SQMS.Models
{
    public class UserInfo
    {
        public bool IsInternal { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string role_name { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string branch_static_ip { get; set; }
        public bool force_change_confirmed { get; set; }
        public string user_login_name { get; set; }
    }

    public class UserInfoV2
    { 
        public bool IsInternal { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string role_id { get; set; }
        public string role_name { get; set; }
        public string password_hash { get; set; }
        public string email { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string branch_static_ip { get; set; }
        public bool force_change_confirmed { get; set; }
        public string user_login_name { get; set; }

    }
}
