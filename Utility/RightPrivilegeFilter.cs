using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace SQMS.Utility
{
    public class RightPrivilegeFilter : ActionFilterAttribute, IActionFilter
    {
        public int PageIds { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Contract.Assert(filterContext != null);

            HttpContext httpContext = filterContext.HttpContext;

            if (filterContext.ActionArguments.ContainsKey(PageIds.ToString()))
            {
                var id = filterContext.ActionArguments[PageIds.ToString()] as int?;
            }

            if (PageIds != 0 && httpContext.Session.GetString("userName") != null)
            {
                string pageIds = httpContext.Session.GetString("PageIds");
                if (!(pageIds.Split(',').Contains(PageIds.ToString())))
                {
                    filterContext.Result = new RedirectToActionResult("Index", "Unauthorized", null);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
