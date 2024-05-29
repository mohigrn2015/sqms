using Microsoft.AspNetCore.Mvc;

namespace SQMS.Controllers
{
    [AuthorizationFilter]
    public class UnauthorizedController : Controller
    {
        // GET: Unauthorized
        public ActionResult Index()
        {
            return View();
        }
    }
}
