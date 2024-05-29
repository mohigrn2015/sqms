namespace SQMS.Models.ViewModels
{
    public class VMPlayList
    {
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }
        public string item_url { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public int duration_in_second { get; set; }
        public int sort_order { get; set; }
    }
}
