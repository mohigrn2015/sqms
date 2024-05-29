using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public string Roles { get; set; } // Property to specify roles

    public AuthorizationFilter() { } // Default constructor

    public AuthorizationFilter(string roles) // Constructor with roles parameter
    {
        Roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

        if (actionDescriptor != null)
        {
            var allowAnonymous = actionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
            if (!allowAnonymous.Any(a => a.GetType() == typeof(AllowAnonymousAttribute)))
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var actionName = actionDescriptor.ActionName;
                    var controllerName = actionDescriptor.ControllerName;

                    if (!(actionName == "ChangePassword" && controllerName == "Manage") &&
                        !(actionName == "LogOff" && controllerName == "Account") &&
                        !(actionName == "SessionOut" && controllerName == "Account"))
                    {
                        if (context.HttpContext.Session.TryGetValue("user_id", out byte[] userIdBytes))
                        {
                            string userRole = context.HttpContext.Session.GetString("userRole");

                            if (context.HttpContext.Session.TryGetValue("IsPasswordExpired", out byte[] passwordExpiredBytes))
                            {
                                bool isPasswordExpired = BitConverter.ToBoolean(passwordExpiredBytes);

                                if (isPasswordExpired && (userRole != "Admin" && userRole != "Branch Admin" && userRole != "Service Holder"))
                                {
                                    context.Result = new RedirectResult("~/Manage/ChangePassword");
                                    return;
                                }
                                else if (context.HttpContext.Session.TryGetValue("ForceChangeConfirmed", out byte[] forceChangeConfirmedBytes))
                                {
                                    bool forceChangeConfirmed = BitConverter.ToBoolean(forceChangeConfirmedBytes);

                                    if (!forceChangeConfirmed && (userRole != "Admin" && userRole != "Branch Admin" && userRole != "Service Holder"))
                                    {
                                        context.Result = new RedirectResult("~/Manage/ChangePassword");
                                        return;
                                    }
                                }
                            }

                            return;
                        }
                        else
                        {
                            context.Result = new RedirectResult("~/Account/SessionOut");
                            return;
                        }
                    }
                }
                else
                {
                    context.Result = new ChallengeResult();
                    return;
                }
            }
        }

        context.Result = new EmptyResult();
    }
}
