using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models.ViewModels;
using SQMS.Utility;
using System.Collections;
using System.Formats.Tar;
using System.IO;

namespace SQMS.Controllers
{
    [AuthorizationFilter(Roles = "Admin,Branch Admin")]
    public class GalleryController : Controller
    {
        private readonly IHttpContextAccessor _session;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GalleryController(IHttpContextAccessor sessionManager,IWebHostEnvironment webHostEnvironment)
        {
            _session = sessionManager;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Gallery
        [RightPrivilegeFilter(PageIds = 800758)]
        public IActionResult Index(string directory = "")
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (string.IsNullOrEmpty(directory))
                {
                    directory = (HttpContext.Request.PathBase == "/" ? "" : HttpContext.Request.PathBase) + ApplicationSetting.galleryDefaultPath;
                    //directory = (Request.PathBase == "/" ? "" : Request.PathBase) + ApplicationSetting.galleryDefaultPath;
                }

                ViewBag.directory = directory;

                string[] patterns = new[] { "*.jpg", "*.jpeg", "*.gif", "*.png", "*.bmp",
                    "*.mpg", "*.mpeg", "*.avi", "*.wmv", "*.mov", "*.rm", "*.ram", "*.swf", "*.flv", "*.ogg", "*.webm", "*.mp4" };

                string pa = Directory.GetCurrentDirectory()+ ApplicationSetting.galleryDefaultPath;

                //string[] files = patterns.SelectMany(pattern =>
                //    Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), ApplicationSetting.galleryDefaultPath), pattern, SearchOption.TopDirectoryOnly)).Distinct().ToArray();

                string[] files = patterns.SelectMany(pattern =>
    Directory.GetFiles(Path.Combine(pa), pattern)).Distinct().ToArray();


                List<VMGalleryItem> gallery = new List<VMGalleryItem>();

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        VMGalleryItem item = new VMGalleryItem() { file_directory = directory, file_name = Path.GetFileName(file) };
                        gallery.Add(item);
                    }
                }
                return View(gallery);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }
        public IActionResult List(string directory = "")
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                //if (string.IsNullOrEmpty(directory))
                //{
                //    directory = ApplicationSetting.galleryDefaultPath;
                //}

                directory = ApplicationSetting.galleryDefaultPath;

                ViewBag.directory = directory;

                string[] patterns = new[] { "*.jpg", "*.jpeg", "*.gif", "*.png", "*.bmp",
                "*.mpg", "*.mpeg", "*.avi", "*.wmv", "*.mov", "*.rm", "*.ram", "*.swf", "*.flv", "*.ogg", "*.webm", "*.mp4" };
                
                string pa = Directory.GetCurrentDirectory() + ApplicationSetting.galleryDefaultPath;

                //string[] files = patterns.SelectMany(pattern =>
                //    Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), ApplicationSetting.galleryDefaultPath), pattern, SearchOption.TopDirectoryOnly)).Distinct().ToArray();

                string[] files = patterns.SelectMany(pattern =>
                        Directory.GetFiles(Path.Combine(pa), pattern)).Distinct().ToArray();

                List<VMGalleryItem> gallery = new List<VMGalleryItem>();

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        VMGalleryItem item = new VMGalleryItem() { file_directory = directory, file_name = Path.GetFileName(file) };
                        gallery.Add(item);
                    }
                }
                return PartialView(gallery);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        public IActionResult Create(string directory = "")
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                if (string.IsNullOrEmpty(directory))
                {
                    directory = (Request.PathBase == "/" ? "" : Request.PathBase) + ApplicationSetting.galleryDefaultPath;
                }
                // If you want to use Request.PathBase, make sure you have the appropriate using directive: using Microsoft.AspNetCore.Http;

                VMGalleryItem vMGalleryItem = new VMGalleryItem() { file_directory = directory };
                return View(vMGalleryItem);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile files)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await files.CopyToAsync(stream);
            }



            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok();
        }


        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            // Get the file from the POST request
            var theFile = HttpContext.Request.Form.Files.GetFile("file");

            // Get the server path, wwwroot
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Building the path to the uploads directory
            var fileRoute = Path.Combine(webRootPath, "uploads");

            // Get the mime type
            var mimeType = HttpContext.Request.Form.Files.GetFile("file").ContentType;

            // Get File Extension
            string extension = System.IO.Path.GetExtension(theFile.FileName);

            // Generate Random name.
            string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

            // Build the full path inclunding the file name
            string link = Path.Combine(fileRoute, name);

            // Create directory if it dose not exist.
            FileInfo dir = new FileInfo(fileRoute);
            dir.Directory.Create();

            // Basic validation on mime types and file extension
            string[] videoMimetypes = { "video/mp4", "video/webm", "video/ogg" };
            string[] videoExt = { ".mp4", ".webm", ".ogg" };

            try
            {
                if (Array.IndexOf(videoMimetypes, mimeType) >= 0 && (Array.IndexOf(videoExt, extension) >= 0))
                {
                    // Copy contents to memory stream.
                    Stream stream;
                    stream = new MemoryStream();
                    theFile.CopyTo(stream);
                    stream.Position = 0;
                    String serverPath = link;

                    // Save the file
                    using (FileStream writerFileStream = System.IO.File.Create(serverPath))
                    {
                        await stream.CopyToAsync(writerFileStream);
                        writerFileStream.Dispose();
                    }

                    // Return the file path as json
                    Hashtable videoUrl = new Hashtable();
                    videoUrl.Add("link", "/uploads/" + name);

                    return Json(videoUrl);
                }
                throw new ArgumentException("The video did not pass the validation");
            }

            catch (ArgumentException ex)
            {
                return Json(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VMGalleryItem galleryItem)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                string directoryPath = Directory.GetCurrentDirectory() + ApplicationSetting.galleryDefaultPath; //Path.Combine(Directory.GetCurrentDirectory(), galleryItem.file_directory);
                if (!Directory.Exists(directoryPath))
                {
                    return Json(new { success = "false", message = "Invalid Gallery URL" });
                }

                var file = galleryItem.file_data;

                var originalFileExtension = Path.GetExtension(file.FileName);

                var newFileName = Path.ChangeExtension(galleryItem.file_name, originalFileExtension);

                string filePath = Path.Combine(directoryPath, newFileName);

                //string filePath = Path.Combine(directoryPath, galleryItem.file_name);
                if (System.IO.File.Exists(filePath))
                {
                    return Json(new { success = "false", message = "A file with this name already exists" });
                }

                if (file != null && file.Length > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);

                    // Example: check if the file extension is allowed
                    if (!IsAllowedFileType(fileExtension))
                    {
                        return Json(new { success = "false", message = "Only video & image files are supported" });
                    }

                    // Example: check if the file size is within limits
                    //if (file.Length > 50000)
                    //{
                    //    return Json(new { success = "false", message = "File size exceeds limit" });
                    //}
                    Stream stream;
                    stream = new MemoryStream();
                    galleryItem.file_data.CopyTo(stream);
                    stream.Position = 0;
                    using (FileStream writerFileStream = System.IO.File.Create(filePath))
                    {
                        await stream.CopyToAsync(writerFileStream);
                        writerFileStream.Dispose();
                    }
                    //using (var stream = System.IO.File.Create(filePath))
                    //{
                    //    await file.CopyToAsync(stream);
                    //}

                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}

                    return RedirectToAction("Index", new { directory = ApplicationSetting.galleryDefaultPath });
                }
                else
                {
                    return View(galleryItem);
                }
            }
            catch (Exception ex)
            {
                // Log the error for diagnosis
                sm.error = ex;
                // Redirect to error handler page
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        private bool IsAllowedFileType(string fileExtension)
        {
            // Example: list of allowed file extensions
            string[] allowedExtensions = { ".mp4", ".mov", ".avi", ".jpg", ".png", ".jpeg", ".gif" };

            return allowedExtensions.Contains(fileExtension);
        }




        //[HttpPost]
        //public IActionResult Create([FromForm] VMGalleryItem galleryItem)
        //{
        //    SessionManager sm = new SessionManager(_session);
        //    try
        //    {
        //        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), galleryItem.file_directory);
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            return Json(new { success = "false", message = "Invalid Gallery URL" });
        //        }

        //        string filePath = Path.Combine(directoryPath, galleryItem.file_name);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            return Json(new { success = "false", message = "A file with this name already exists" });
        //        }

        //        var file = galleryItem.file_data;

        //        if (file != null && file.Length > 0)
        //        {
        //            var fileExtension = Path.GetExtension(file.FileName);

        //            if (MediaContentManager.FileType(fileExtension) == "")
        //            {
        //                return Json(new { success = "false", message = "Only video & image files are supported" });
        //            }

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            return RedirectToAction("Index", new { directory = ApplicationSetting.galleryDefaultPath });
        //        }
        //        else
        //        {
        //            return View(galleryItem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sm.error = ex;
        //        return RedirectToAction("Index", "ErrorHandler");
        //    }
        //}

        //[HttpPost]
        //public IActionResult Create([FromForm]VMGalleryItem galleryItem)
        //{
        //    SessionManager sm = new SessionManager(_session);
        //    try
        //    {
        //        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), galleryItem.file_directory);
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            return Json(new { success = "false", message = "Invalid Gallery URL" });
        //        }

        //        string filePath = Path.Combine(directoryPath, galleryItem.file_name);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            return Json(new { success = "false", message = "One file already exists with this file name" });
        //        }

        //        var file = galleryItem.file_data;

        //        if (file != null && file.Length > 0)
        //        {
        //            // extract only the filename
        //            var fileExtension = Path.GetExtension(file.FileName);

        //            if (MediaContentManager.FileType(fileExtension) == "")
        //            {
        //                return Json(new { success = "false", message = "Only Video & Image file support" });
        //            }

        //            // store the file inside ~/App_Data/uploads folder
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            return RedirectToAction("Index", new { directory = ApplicationSetting.galleryDefaultPath });
        //        }
        //        else
        //        {
        //            return View(galleryItem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sm.error = ex;
        //        return RedirectToAction("Index", "ErrorHandler");
        //    }
        //}

        [HttpPost]
        public IActionResult Delete(string fileFullPath)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileFullPath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    var usedList = new BLLPlayListItem().GetByFileName(Path.GetFileName(fileFullPath));
                    if (usedList.Count == 1)
                    {
                        return Ok(new { success = false, message = "File is used in " + usedList[0].playlist_name + " playlist" });
                    }
                    else if (usedList.Count > 1)
                    {
                        return Ok(new { success = false, message = "File is used in multiple playlists" });
                    }
                    else
                    {
                        System.IO.File.Delete(filePath);
                        return Ok(new { success = true, message = "Successfully deleted." });
                    }
                }
                else
                {
                    return Ok(new { success = false, message = "File not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
    }
}
