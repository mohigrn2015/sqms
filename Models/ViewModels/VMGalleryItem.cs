namespace SQMS.Models.ViewModels
{
    public class VMGalleryItem
    {

        public string file_directory { get; set; }
        public string file_name { get; set; }

        public IFormFile file_data { get; set; }

        public string file_full_path
        {
            get
            {
                try
                {
                    if (file_type == "VIDEO")
                        return file_directory.Replace("~", "") + (file_directory.Last() == '/' ? "" : "/") + file_name;
                    else
                        return file_directory.Replace("~", "") + (file_directory.Last() == '/' ? "" : "/") + file_name;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public string file_extenstion
        {
            get
            {
                return file_name.Split('.').LastOrDefault();
            }
        }

        public string file_type
        {
            get
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
}
