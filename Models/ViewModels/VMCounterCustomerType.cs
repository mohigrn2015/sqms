namespace SQMS.Models.ViewModels
{
    public class VMCounterCustomerType
    {
        public int counter_customer_type_id { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public int counter_id { get; set; }
        public string counter_no { get; set; }
        public int customer_type_id { get; set; }
        public string customer_type_name { get; set; }
        public int? is_active { get; set; }
    }
}
