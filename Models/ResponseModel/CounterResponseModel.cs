using SQMS.Models.ViewModels;
using SQMS.Utility;

namespace SQMS.Models.ResponseModel
{
    public class CounterResponseModel
    {
        public bool success { get; set; }
        public List<VMTokenProgress> tokenInProgress { get; set; }
        public string nextTokens { get; set; }
        public string dispalyVideoUrl { get; set; }       
    }
}
