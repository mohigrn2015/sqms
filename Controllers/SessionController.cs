using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;

namespace SQMS.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public bool ForceChangeConfirmedGet()
        {
            bool ForceChangeConfirmed = Convert.ToBoolean(HttpContext.Session.GetString("ForceChangeConfirmed"));
            return ForceChangeConfirmed;
        }
        public void ForceChangeConfirmedSet(string value)
        {
            HttpContext.Session.SetString("userName", value);
            HttpContext.Session.SetString("ForceChangeConfirmed", value);
        }
        public void DoSomething(HttpContext httpContext)
        {
            // Access session data using HttpContext
            var forceChangeConfirmed = httpContext.Session.GetString("ForceChangeConfirmed");
            // Other session-related operations...
        }
    }
}
