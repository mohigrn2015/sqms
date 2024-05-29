namespace SQMS.Models.ViewModels
{
    public class VMServiceType
    {
        public int service_sub_type_id { get; set; }
        public string service_type_name { get; set; }
        public string service_sub_type_name { get; set; }
        public int max_duration { get; set; }
        public int is_active { get; set; }
        public int tat_warning_time { get; set; }

    }
}
