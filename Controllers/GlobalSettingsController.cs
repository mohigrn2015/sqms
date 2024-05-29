using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQMS.BLL;
using SQMS.Models;
using SQMS.Utility;

namespace SQMS.Controllers
{
    //[AuthorizationFilter(Roles = "Admin")]
    public class GlobalSettingsController : Controller
    {
        private BLLGlobalSettings BLLGlobalSettings = new BLLGlobalSettings();
        private readonly IHttpContextAccessor _session;
        public GlobalSettingsController(IHttpContextAccessor httpContext)
        {
             _session = httpContext;
        }
        // GET: GlobalSettings
        //[RightPrivilegeFilter(PageIds = 800765)]
        public IActionResult Index()
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                tblGlobalSettings globalSettings = BLLGlobalSettings.Get();
                List<SelectListItem> configTimeList = new List<SelectListItem>();
                for (int i = 1; i <= 10; i++)
                {
                    var list = new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    };
                    configTimeList.Add(list);
                }

                List<SelectListItem> notificationVisibilityDaysList = new List<SelectListItem>();
                for (int i = 15; i <= 180; i += 15)
                {
                    var list = new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    };
                    notificationVisibilityDaysList.Add(list);
                }
                string sep_char = "~#,^";
                List<SelectListItem> separetorList = new List<SelectListItem>();
                for (int i = 0; i < 4; i++)
                {
                    var list = new SelectListItem
                    {
                        Text = sep_char.Substring(i, 1),
                        Value = sep_char.Substring(i, 1)
                    };
                    separetorList.Add(list);
                }
                ViewBag.PopUpDuration = configTimeList;
                ViewBag.NotificationVisibilityDaysList = notificationVisibilityDaysList;
                ViewBag.separetorList = separetorList;

                return View(globalSettings);
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        public IActionResult Edit(int tat_visibility_time, int notification_visibility_days, bool is_msg_bn,
                                    string default_token_prefix, int padding_left, string msg_text,
                                    string display_footer_ad, string report_csv_separator)
        {
            SessionManager sm = new SessionManager(_session);
            try
            {
                tblGlobalSettings tblGlobalSettings = new tblGlobalSettings()
                {
                    tat_visibility_time = tat_visibility_time,
                    notification_visibility_days = notification_visibility_days,
                    is_msg_bn = is_msg_bn,
                    default_token_prefix = default_token_prefix,
                    padding_left = padding_left,
                    msg_text = msg_text,
                    display_footer_ad = display_footer_ad,
                    report_csv_separator = report_csv_separator
                };
                BLLGlobalSettings.Edit(tblGlobalSettings);
                return RedirectToAction("AdminDashboard", "Home");
            }
            catch (Exception ex)
            {
                sm.error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }            
        }
    }
}
