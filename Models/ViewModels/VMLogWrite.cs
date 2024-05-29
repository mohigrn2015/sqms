namespace SQMS.Models.ViewModels
{
    public class VMLogWrite
    {
        private string _error_occured_at;
        public bool is_error { get; set; }
        public string error_source { get; set; }
        public string class_name { get; set; }
        public string method_name { get; set; }
        public string error_message { get; set; }
        public string error_occured_at { get; set; }
    }
}
