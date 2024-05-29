namespace SQMS.Models
{
    public class ValidateResModel
    {
        public int executionState { get; set; }
        public bool is_success { get; set; }
        public string message { get; set; }
    }

    public class ValidateUserData
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string token { get; set; }
    }
}
