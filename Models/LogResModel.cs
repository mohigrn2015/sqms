namespace SQMS.Models
{
    public class LogResModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int user_id { get; set; }
        public string user_type { get; set; }
        public string allocated_center { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int? center_id { get; set; }

    }
}
