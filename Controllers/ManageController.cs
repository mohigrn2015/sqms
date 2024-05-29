using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Helper;
using SQMS.Models;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;
using SQMS.Utility;
using System.Security.Claims;

namespace SQMS.Controllers
{
    public class ManageController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _session;
        private readonly CustomUserManager<IdentityUser> _customUserManager;
        public ManageController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IHttpContextAccessor session, CustomUserManager<IdentityUser> customUserManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _session = session;
            _customUserManager = customUserManager;
        }

        public async Task<IActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;

            var userData = await _userManager.GetUserAsync(User);

            var result = await _userManager.RemoveLoginAsync(userData, loginProvider, providerKey);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(userData.Id);

                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userData = await _userManager.GetUserAsync(User);

            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userData, model.Number);

            //var message = new IdentityMessage
            //{
            //    Destination = model.Number,
            //    Body = "Your security code is: " + code
            //};

            //if (_userManager.SmsService != null)
            //{
            //    var message = new IdentityMessage
            //    {
            //        Destination = model.Number,
            //        Body = "Your security code is: " + code
            //    };
            //    await UserManager.SmsService.SendAsync(message);
            //}
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            try
            {
                var userData = await _userManager.GetUserAsync(User);

                await _userManager.SetTwoFactorEnabledAsync(userData, true);

                if (userData != null)
                {
                    await _signInManager.SignInAsync(userData, isPersistent: false);
                }
            }
            catch (Exception)
            {
                throw;
            }            
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            }
            catch (Exception)
            {
                throw;
            }
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userData = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePhoneNumberAsync(userData, model.PhoneNumber, model.Code);
           
            if (result.Succeeded)
            {
                
                if (userData != null)
                {
                    await _signInManager.SignInAsync(userData, false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.SetPhoneNumberAsync(user, null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userData = await _userManager.FindByIdAsync(_userManager.GetUserId(User));

                //var result = await _userManager.ChangePasswordAsync(userData, model.OldPassword, model.NewPassword);
                var result = await _customUserManager.ChangePasswordAsync(userData, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    if (userData != null)
                    {
                        await _signInManager.SignInAsync(userData, isPersistent: false);
                    }
                    SessionManager sm = new SessionManager(_session);
                    VMSessionInfo userInfo = new BLLAspNetUser(_session).GetSessionInfoByUserName(userData.UserName);
                    if (userInfo.role_name == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Home");
                    }
                    else if (userInfo.role_name == "Branch Admin")
                    {
                        return RedirectToAction("BranchSelection", "Home");
                    }
                    else if (userInfo.role_name == "Token Generator")
                    {
                        return RedirectToAction("Create", "TokenQueues");
                    }
                    else if (userInfo.role_name == "Service Holder")
                    {
                        return RedirectToAction("CounterSelection", "Home");
                    }
                    else if (userInfo.role_name == "Display User")
                    {
                        return RedirectToAction("CounterList", "Counters");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                AddErrors(result);
            }
            catch (Exception)
            {
                throw;
            }
           
            return View(model);
        }
        public IActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = await _userManager.GetUserAsync(User);

                    var result = await _userManager.AddPasswordAsync(userData, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                        if (user != null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    AddErrors(result);
                }
            }
            catch (Exception)
            {
                throw;
            }            

            return View(model);
        }

        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            try
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                    : message == ManageMessageId.Error ? "An error has occurred."
                    : "";

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Error");
                }

                var userLogins = await _userManager.GetLoginsAsync(user);
                var otherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                    .Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider))
                    .ToList();

                ViewBag.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || userLogins.Count > 1;

                return View(new ManageLoginsViewModel
                {
                    CurrentLogins = userLogins,
                    OtherLogins = otherLogins
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, provider);
        }

        public async Task<IActionResult> LinkLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }

            var result = await _userManager.AddLoginAsync(await _userManager.GetUserAsync(User), info);
            if (result.Succeeded)
            {
                return RedirectToAction("ManageLogins");
            }
            else
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
            }

            base.Dispose(disposing);
        }

        private const string XsrfKey = "XsrfId";
               
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
            }
        }

        private async Task<bool> HasPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.PasswordHash != null;
        }
        
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
    }
}
