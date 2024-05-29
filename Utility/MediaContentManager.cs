namespace SQMS.Utility
{
    public class MediaContentManager
    {
        public static string FileType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".mpg":
                case ".mpeg":
                case ".avi":
                case ".wmv":
                case ".mov":
                case ".rm":
                case ".ram":
                case ".swf":
                case ".flv":
                case ".ogg":
                case ".webm":
                case ".mp4":
                    return "VIDEO";
                case ".jpeg":
                case ".jpg":
                case ".png":
                case ".gif":
                case ".bmp":
                    return "IMAGE";
                default:
                    return "";
            }

        }
    }
}
