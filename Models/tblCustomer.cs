namespace SQMS.Models
{
    public partial class tblCustomer
    {
        public tblCustomer()
        {
            this.tblServiceDetails = new HashSet<tblServiceDetail>();
        }

        public long customer_id { get; set; }
        public string customer_name { get; set; }
        public string contact_no { get; set; }
        public string address { get; set; }
        public int customer_type_id { get; set; }
        public virtual ICollection<tblServiceDetail> tblServiceDetails { get; set; }
        public virtual tblCustomerType tblCustomerType { get; set; }
    }
}
