using System.ComponentModel.DataAnnotations;

namespace SQMS.Models.MetaModels
{
    [MetadataType(typeof(PlayListItemMeta))]
    public partial class tblPlayListItem
    {
        public string file_name { get; set; }
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

    public class PlayListItemMeta
    {

        [Required]
        public int playlist_id { get; set; }
        [Required]
        [Display(Name = "URL")]
        public string item_url { get; set; }
        [Required]
        [Display(Name = "File Name")]
        public string file_name { get; set; }
        [Display(Name = "File Type")]
        public string file_type { get; set; }
        [Required]
        [Display(Name = "Duration (Second)")]
        public int duration_in_second { get; set; }
        [Required]
        [Display(Name = "Sort Order")]
        public int sort_order { get; set; }


    }
}
