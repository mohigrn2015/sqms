namespace SQMS.Models.ResponseModel
{
    public class CommonResponseModel
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class CommonLogModel
    {
        public object model { get; set; }
        public object message { get; set; }
    }
    public class CommonLogModelv2
    {
        public object model { get; set; }
        public object message { get; set; }
        public object message2 { get; set; }
    }
}
