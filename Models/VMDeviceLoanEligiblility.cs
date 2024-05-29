namespace SQMS.Models
{
    public class VMDeviceLoanEligiblility
    {
        public int id { get; set; }
        public string msisdn { get; set; }
        public DateTime? loan_date { get; set; }
        public bool is_eligible { get; set; }
    }
}
