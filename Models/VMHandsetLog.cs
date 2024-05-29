namespace SQMS.Models
{
    public class VMHandsetLog
    {
        public int id { get; set; }
        public string msisdn { get; set; }
        public DateTime request_date { get; set; }
        public byte[] request_data { get; set; }
        public DateTime response_date { get; set; }
        public byte[] response_data { get; set; }
        public int api_status { get; set; }
        public string message { get; set; }
    }
}

