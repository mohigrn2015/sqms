using SQMS.Models.ViewModels;

namespace SQMS.Models.ResponseModel
{
    public class DisplayInfoResponseModel
    {
        public bool success { get; set; }
        public List<VMTokenProgress> tokenInProgress { get; set; }
        public string nextTokens { get; set; }
        public int playlist_id { get; set; }
        public VMDisplayFooter displayFooter { get; set; }    
    }

    public class CounterDisplayInfoResponseModel
    {
        public bool success { get; set; } 
        public List<VMTokenProgress> tokenInProgress { get; set; }
        public string nextTokens { get; set; }
    }
     
    public class NextTokenResponseModel
    {
        public bool success { get; set; }
        public string nextTokens { get; set; }
    }
    public class PlayListResponseModel
    {
        public bool success { get; set; }
        public List<VMPlayList> playLists { get; set; }
    }
    public class DisplayResponseModel
    { 
        public bool success { get; set; }
        public VMDisplayFooter displayFooter { get; set; }
    }
}