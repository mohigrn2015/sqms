using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public partial class tblPlayListItem
    {
        [Key]
        public int playlistitem_id { get; set; }
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }
        public string item_url { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public int duration_in_second { get; set; }
        public int sort_order { get; set; }

        public string file_extenstion
        {
            get
            {
                return file_name.Split('.').LastOrDefault();
            }
        }

        public string getFileType()
        {

            switch (file_extenstion.ToLower())
            {
                case "mpg":
                case "mpeg":
                case "avi":
                case "wmv":
                case "mov":
                case "rm":
                case "ram":
                case "swf":
                case "flv":
                case "ogg":
                case "webm":
                case "mp4":
                    return "VIDEO";
                case "jpeg":
                case "jpg":
                case "png":
                case "gif":
                case "bmp":
                    return "IMAGE";
                default:
                    return "";
            }
        }
    }
}
