using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using SQMS.Utility;

namespace SQMS.Controllers
{
    public class ErrorHandlerController : Controller
    {
        private readonly IHttpContextAccessor _session;
        public ErrorHandlerController(IHttpContextAccessor httpContext)
        {
            _session = httpContext;
        }
        // GET: ErrorHandler
        public IActionResult Index(string extra = "")
        {

            Exception ex = new SessionManager(_session).error;
            if (extra.Length > 0)
                ViewBag.ExtraErrorMessage = Cryptography.Decrypt(extra, true);
            else
                ViewBag.ExtraErrorMessage = "";
            ViewBag.ErrorMessage = extra + "\n\n" + ex.Message;
            ViewBag.InnerErrorMessage = ex.Data.ToString() + ex.InnerException + ex.TargetSite + ex.StackTrace + ex.Source + ex.HResult;
            return View();
        }
    }
}
