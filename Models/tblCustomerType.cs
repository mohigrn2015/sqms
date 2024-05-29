namespace SQMS.Models
{
    public partial class tblCustomerType
    {
        public tblCustomerType()
        {
            this.tblCustomers = new HashSet<tblCustomer>();
        }

        public int customer_type_id { get; set; }
        public string customer_type_name { get; set; }

        public int priority { get; set; }

        public string token_prefix { get; set; }

        public int is_default { get; set; }

        public virtual ICollection<tblCustomer> tblCustomers { get; set; }
    }
}
