namespace SQMS.Models
{
    public class UserInfoModel
    {
        public int user_id { get; set; }
        public string user_status { get; set; }
        public string current_center { get; set; }
        public int center_id { get; set; }
        public string is_locked { get; set; }
        public string is_internal { get; set; }
        public string role_name { get; set; }
        public string hash_password { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int rccm_id { get; set; }
        public int zccm_id { get; set; }
        public int ccr_id { get; set; }
    }
}
