using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using SQMS.BLL;
using SQMS.Models.ViewModels;
using SQMS.Models;
using SQMS.Utility;
using SQMS.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Text;
using SQMS.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SQMS.SignalRHub;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.AspNetCore.SignalR;
using SQMS.Models.ResponseModel;
using SQMS.Models.RequestModel;

namespace SQMS.Controllers
{
    public class AccountController : Controller
    {
        private BLL.BLLBranch dbBranch = new BLL.BLLBranch();
        private BLL.BLLAspNetUser dbUser = new BLL.BLLAspNetUser();
        private BLL.BLLAspNetRole dbRoles = new BLL.BLLAspNetRole();
        private BLL.BLLBranchUsers dbBranchUser = new BLL.BLLBranchUsers();
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomSignInManager _customSignInManager;
        private readonly IHttpContextAccessor _session;
        private readonly IHubContext<notifyDisplay> _hubContext;
        private readonly CustomUserManager<IdentityUser> _customUserManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, CustomSignInManager customSignInManager, IHttpContextAccessor httpContextAccessor, IHubContext<notifyDisplay> hubContext, CustomUserManager<IdentityUser> customUserManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _customSignInManager = customSignInManager;
            _session = httpContextAccessor;
            _hubContext = hubContext;
            _customUserManager = customUserManager;
        }

        [HttpGet]
        public IActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();

            var identity = new ClaimsIdentity(claims, "Bearer");

            HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                    if (User.IsInRole("Admin"))
                    {
                        if (HttpContext.Session.GetString("PageIds") != null)
                        {
                            return RedirectToAction("Index", "Unauthorized");
                        }

                        return RedirectToAction("AdminDashboard", "Home");
                    }
                    else if (User.IsInRole("Branch Admin"))
                    {
                        if (HttpContext.Session.GetString("PageIds") != null)
                        {
                            return RedirectToAction("Index", "Unauthorized");
                        }
                        return RedirectToAction("BranchAdminDashboard", "Home");
                    }
                    else if (User.IsInRole("Token Generator"))
                    {
                        return RedirectToAction("Create", "TokenQueues");
                    }
                    else if (User.IsInRole("Service Holder"))
                    {
                        return RedirectToAction("CounterSelection", "Home");
                    }
                    else if (User.IsInRole("Display User"))
                    {
                        return RedirectToAction("CounterList", "Counters");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult DeviceLogin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                SessionManager sm = new SessionManager(_session);
                if (sm.IsPasswordExpired || sm.ForceChangeConfirmed == false)
                {
                    return RedirectToAction("ChangePassword", "Manage");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                else if (User.IsInRole("Branch Admin"))
                {
                    return RedirectToAction("BranchAdminDashboard", "Home");
                }
                else if (User.IsInRole("Token Generator"))
                {
                    return RedirectToAction("Create", "TokenQueues");
                }
                else if (User.IsInRole("Service Holder"))
                {
                    return RedirectToAction("CounterSelection", "Home");
                }
                else if (User.IsInRole("Display User"))
                {
                    return RedirectToAction("CounterList", "Counters");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                AspNetUserLoginAttempts loginAttempt = new AspNetUserLoginAttempts()
                {
                    UserName = model.UserName,
                    is_success = 0,
                    ip_address = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    machine_name = DetermineCompName(HttpContext.Connection.RemoteIpAddress?.ToString())
                };
                //dbUser.AddLoginAttemptInfo(loginAttempt);

                VMBranchLogin branchLogin = new BLLBranchUsers().GetAll().Where(w => w.UserName == model.UserName).FirstOrDefault();

                UserInfo userInfo = new WebLoginBLL().IsUserExist(model.UserName);

                if (branchLogin == null && userInfo == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

                if (ApplicationSetting.AllowActiveDirectoryUser)
                {
                    if (branchLogin != null)
                    {
                        if (await CheckActiveDirectoryLogin(model.UserName, model.Password))
                        {
                            model.Password = "Ssl@1234";
                        }
                        else
                        {
                            loginAttempt.is_success = 0;
                            dbUser.AddLoginAttemptInfo(loginAttempt);
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }
                    }
                    else if (userInfo != null)
                    {
                        if (await CheckActiveDirectoryLogin(model.UserName, model.Password))
                        {
                            model.Password = "Ssl@1234";
                        }
                        else
                        {
                            loginAttempt.is_success = 0;
                            dbUser.AddLoginAttemptInfo(loginAttempt);
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }
                    }
                }
                else
                {
                    int passwordPattern = userInfo.user_id.Split('-').Length;
                    if (passwordPattern == 1)
                    {
                        model.Password = "Ssl@1234";
                    }
                }

                User user1 = new User();
                user1.UserName = model.UserName;
                user1.PasswordHash = model.Password;

                var result = await _customSignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    SessionManager sm = new SessionManager(_session);
                    BLL.BLLAspNetUser dbUserWithSession = new BLL.BLLAspNetUser(_session);
                    VMSessionInfo user = dbUserWithSession.GetSessionInfoByUserName(model.UserName);

                    if (user.role_name.ToLower() == ("Branch Admin").ToLower() && (branchLogin != null && branchLogin.branch_id == 0))
                    {
                        await _signInManager.SignOutAsync();
                        sm.Close();
                        ModelState.AddModelError("", "No assigned branch found.");
                        return View(model);
                    }

                    List<string> rightInformations = new WebLoginBLL().GetRightInformationList(model.UserName).Select(x => x.PAGE_ID.ToString()).ToList();

                    HttpContext.Session.SetString("PageIds", string.Join(",", rightInformations));
                    HttpContext.Session.SetString("userName", model.UserName);
                    HttpContext.Session.SetString("userRole", user.role_name);

                    string loginProvider = Guid.NewGuid().ToString();
                    string securityToken = Cryptography.Encrypt(loginProvider, true);
                    AspNetUserLogin login = new AspNetUserLogin()
                    {
                        LoginProvider = loginProvider,
                        ProviderKey = securityToken,
                        UserId = user.user_id,
                        logout_reason = ""
                    };

                    sm.user_name = user.user_name;
                    sm.user_id = user.user_id;
                    sm.LoginProvider = loginProvider;
                    if (user.branch_id > 0)
                    {
                        login.branch_id = user.branch_id;
                        if (!user.role_name.Equals("Branch Admin"))
                        {
                            sm.branch_id = user.branch_id;
                            sm.branch_name = user.branch_name;
                            sm.branch_static_ip = user.branch_static_ip;
                        }
                    }
                    else
                    {
                        sm.branch_id = 0;
                    }
                    sm.IsActiveDirectoryUser = true; // (branchLogin.is_active_directory_user > 0);

                    dbUser.AddLoginInfo(login);

                    loginAttempt.is_success = 1;
                    //loginAttempt.machine_name = "3";
                    loginAttempt.LoginProvider = loginProvider;
                    dbUser.AddLoginAttemptInfo(loginAttempt);



                    if ((sm.IsPasswordExpired || sm.ForceChangeConfirmed == false) && (user.role_name == "Display User" || user.role_name == "Token Generator"))
                    {
                        return RedirectToAction("ChangePassword", "Manage");
                    }
                    else if (user.role_name == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Home");
                    }
                    else if (user.role_name == "Branch Admin")
                    {
                        return RedirectToAction("BranchSelection", "Home");
                        //return RedirectToAction("BranchAdminDashboard", "Home");
                    }
                    else if (user.role_name == "Token Generator")
                    {
                        return RedirectToAction("Create", "TokenQueues");
                    }
                    else if (user.role_name == "Service Holder")
                    {
                        return RedirectToAction("CounterSelection", "Home");
                    }
                    else if (user.role_name == "Display User")
                    {
                        return RedirectToAction("CounterList", "Counters");
                        //return RedirectToAction("Index", "Counters");
                    }
                    return RedirectToLocal(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    loginAttempt.is_success = 0;
                    dbUser.AddLoginAttemptInfo(loginAttempt);
                    return View("Lockout");
                }
                else
                {
                    loginAttempt.is_success = 0;
                    dbUser.AddLoginAttemptInfo(loginAttempt);
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                new SessionManager(_session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        }

        private async Task<HttpResponseMessage> CallJsonAPI(string url, dynamic model)
        {
            var jsonRequest = string.Empty;
            var jsonResponse = string.Empty;

            try
            {
                jsonRequest = JsonConvert.SerializeObject(model);
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpResponseMessage result = new HttpResponseMessage();

                HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                var uri = new Uri(url, UriKind.Absolute);

                result = client.PostAsync(uri, content).Result;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class BaseUri
        {
            public Uri baseAddress = new Uri(ApplicationSetting.ADAPI);
            public HttpClient client;
            public BaseUri()
            {
                client = new HttpClient();
                client.BaseAddress = baseAddress;
            }
        }
        private async Task<bool> CheckActiveDirectoryLogin(string userName, string password)
        {
            BLLLog log = new BLLLog();
            string user = userName.Replace("'", "#").Trim();
            bool result = false;
            BaseUri baseAdd = new BaseUri();
            dynamic credentials = new ExpandoObject();
            credentials.applicationName = ApplicationSetting.ADUser;
            credentials.applicationKey = ApplicationSetting.ADPassword;
            credentials.userName = userName;
            credentials.password = password;
            credentials.rememberMe = false;

            try
            {
                //var authLogin = await CallJsonAPI("http://172.16.7.181:8080/account/login", credentials);
                var authLogin = await CallJsonAPI(StaticConfigValue.GetBlAuthenticationAPI(), credentials);
                var responseContent = authLogin.Content.ReadAsStringAsync().Result;
                ValidateResModel apiResponse2 = string.IsNullOrWhiteSpace(responseContent) ? null : JsonConvert.DeserializeObject<ValidateResModel>(responseContent);

                result = apiResponse2.is_success;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

            var model = new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeviceLogin(LoginViewModel model, string returnUrl)
        {
            BLL.BLLAspNetUser dbUser = new BLL.BLLAspNetUser(_session);
            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password))
            {
                return View(model);
            }
            AspNetUserLoginAttempts loginAttempt = new AspNetUserLoginAttempts()
            {
                UserName = model.UserName,
                is_success = 0
            };

            VMBranchLogin branchLogin = new BLLBranchUsers().GetAll().Where(w => w.UserName == model.UserName).FirstOrDefault();
            if (branchLogin != null)
            {
                if (branchLogin.is_active_directory_user > 0)
                {
                    //Check active directory login
                    if (await CheckActiveDirectoryLogin(model.UserName, model.Password))
                    {
                        model.Password = "Ssl@1234";
                    }
                    else
                    {
                        loginAttempt.is_success = 0;
                        dbUser.AddLoginAttemptInfo(loginAttempt);
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }
                }
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                SessionManager sm = new SessionManager(_session);
                VMSessionInfo user = dbUser.GetSessionInfoByUserName(model.UserName);

                string loginProvider = Guid.NewGuid().ToString();
                string securityToken = Cryptography.Encrypt(loginProvider, true);
                AspNetUserLogin login = new AspNetUserLogin()
                {
                    LoginProvider = loginProvider,
                    ProviderKey = securityToken,
                    UserId = user.user_id,
                    logout_reason = ""
                };

                sm.user_name = user.user_name;
                sm.user_id = user.user_id;
                sm.LoginProvider = loginProvider;
                if (user.branch_id > 0)
                {
                    login.branch_id = user.branch_id;
                    if (!user.role_name.Equals("Branch Admin"))
                    {
                        sm.branch_id = user.branch_id;
                        sm.branch_name = user.branch_name;
                        sm.branch_static_ip = user.branch_static_ip;
                    }
                }
                else
                {
                    sm.branch_id = 0;
                }
                sm.IsActiveDirectoryUser = (branchLogin.is_active_directory_user > 0);

                dbUser.AddLoginInfo(login);

                loginAttempt.is_success = 1;
                loginAttempt.LoginProvider = loginProvider;
                dbUser.AddLoginAttemptInfo(loginAttempt);

                if ((sm.IsPasswordExpired || sm.ForceChangeConfirmed == false) && (user.role_name == "Display User" || user.role_name == "Token Generator"))
                {
                    return RedirectToAction("ChangePassword", "Manage");
                }
                else if (user.role_name == "Admin")
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                else if (user.role_name == "Branch Admin")
                {
                    return RedirectToAction("BranchSelection", "Home");
                }
                else if (user.role_name == "Token Generator")
                {
                    return RedirectToAction("Create", "TokenQueues");
                }
                else if (user.role_name == "Service Holder")
                {
                    return RedirectToAction("CounterSelection", "Home");
                }
                else if (user.role_name == "Display User")
                {
                    return RedirectToAction("CounterList", "Counters");
                }

                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                loginAttempt.is_success = 0;
                dbUser.AddLoginAttemptInfo(loginAttempt);
                return View("Lockout");
            }
            else if (result.IsNotAllowed)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
            }
            else
            {
                loginAttempt.is_success = 0;
                dbUser.AddLoginAttemptInfo(loginAttempt);
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
        }

        private string DetermineCompName(string IP)
        {
            try
            {
                IPAddress myIP = IPAddress.Parse(IP);
                IPHostEntry hostEntry = Dns.GetHostEntry(myIP);
                string hostName = hostEntry.HostName;
                return hostName.Split('.').FirstOrDefault() ?? "Not Found";
            }
            catch (Exception)
            {
                return "Not Found";
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OuterLogin([FromBody] OuterLoginRequestModel outerLogin)
        {
            BLL.BLLAspNetUser dbUser = new BLL.BLLAspNetUser(_session);
            OuterLoginResponseModel responseModel = new OuterLoginResponseModel();
            string responseJson = String.Empty;
            string securityToken = outerLogin.username;          
            try
            {
                var result2 = await _signInManager.PasswordSignInAsync(outerLogin.username, outerLogin.password, false, lockoutOnFailure: true);
                
                if (result2.Succeeded)
                {
                    SessionManager sm = new SessionManager(_session);
                    VMSessionInfo user = dbUser.GetSessionInfoByUserName(outerLogin.username);

                    string loginProvider = Guid.NewGuid().ToString();
                    securityToken = Cryptography.Encrypt(loginProvider, true);
                    AspNetUserLogin login = new AspNetUserLogin()
                    {
                        LoginProvider = loginProvider,
                        ProviderKey = securityToken,
                        UserId = user.user_id,
                        logout_reason = ""
                    };

                    sm.user_name = user.user_name;
                    sm.user_id = user.user_id;
                    sm.LoginProvider = loginProvider;
                    if (user.branch_id > 0)
                    {
                        sm.branch_id = login.branch_id = user.branch_id;
                        sm.branch_name = user.branch_name;
                        sm.branch_static_ip = user.branch_static_ip;
                    }
                    else
                    {
                        sm.branch_id = 0;
                    }

                    dbUser.AddLoginInfo(login);

                    BLLServiceType dbServiceType = new BLLServiceType();

                    List<ServiceListModel> serviceList = dbServiceType.GetAll().Select(s => new ServiceListModel { service_type_id = s.service_type_id, service_type_name = s.service_type_name }).ToList();

                    responseModel = new OuterLoginResponseModel()
                    {
                        success = true,
                        message = "login success",
                        branch_id = sm.branch_id,
                        securityToken = securityToken,
                        server_url = user.branch_static_ip,
                        serviceList = serviceList,
                        appRequestTimeOut = ApplicationSetting.AppRequestTimeOut
                    };

                    return Ok(responseModel);
                }
                else if (result2.IsLockedOut)
                {
                    InvalidResponse invalidResponse = new InvalidResponse()
                    {
                        success = false,
                        message = "User is locked, please contact to admin"
                    };
                    return Ok(invalidResponse);
                }
                else if (result2.IsNotAllowed)
                {
                    InvalidResponse invalidResponse = new InvalidResponse()
                    {
                        success = false,
                        message = "User is not verified, please contact to admin"
                    };
                    return Ok(invalidResponse);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                    
                    InvalidResponse invalidResponse = new InvalidResponse()
                    {
                        success = false,
                        message = "Invalid login attempt"
                    };
                    return Ok(invalidResponse);
                }
            }
            catch (Exception ex)
            {
                InvalidResponse invalidResponse = new InvalidResponse()
                {
                    success = false,
                    message = ex.Message
                };
                return Ok(invalidResponse);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // GET: /Account/Register
        [AuthorizationFilter(Roles = "Admin,Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800707)]
        public IActionResult Register(string type)
        {
            RegisterViewModel register = new RegisterViewModel();
            register.is_active_directory_user = 0;
            if (type == "Token")
            {
                register.name = "Token Generator";
                //ViewBag.name = new SelectList(dbRoles.GetAllRoles().Where(e => e.Name != "Admin"), "name", "name", "Token Generator");
                ViewBag.name = dbRoles.GetAllRoles().Where(e => e.Name == register.name);
            }
            else if (type == "Display")
            {
                register.name = "Display User";
                //ViewBag.name = new SelectList(dbRoles.GetAllRoles().Where(e => e.Name != "Admin"), "name", "name", "Display User");
                ViewBag.name = dbRoles.GetAllRoles().Where(e => e.Name == register.name);
            }

            int branch_id = new SessionManager(_session).branch_id;
            ViewBag.branchList = dbBranch.GetAllBranch();
            ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", branch_id); ///----Kamrul

            ViewBag.is_active_directory_user = DropDownListManager.GetUserType();

            return View(register);
        }

        // POST: /Account/Register
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin,Branch Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password))
            {
                ViewBag.branchList = dbBranch.GetAllBranch();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Hometown = model.Hometown,
                PhoneNumber = model.Mobile
            };
            user.LockoutEnd = DateTime.UtcNow; // Consider using UtcNow instead of Now
            int branch_id = 0;
            if (!User.IsInRole("Admin"))
            {
                branch_id = new SessionManager(_session).branch_id;
            }
            else
            {
                branch_id = model.branch_id;
            }

            try
            {
                if (model.is_active_directory_user > 0)
                {
                    if (!CheckActiveDirectory(model.UserName))
                    {
                        ModelState.AddModelError("", "User not found in Active Directory.");
                        return View(model);
                    }
                    else
                    {
                        model.ConfirmPassword = model.Password = "Ssl@1234";
                        ModelState.Clear();
                    }
                }
                else
                {
                    if (model.Password.ToUpper().Contains(model.UserName.ToUpper()))
                    {
                        ModelState.AddModelError("", "Password must not contain User Name.");
                        return View(model);
                    }
                    Regex regex = new Regex(@"^[a-zA-Z0-9.\s]+$");
                    if (!regex.Match(model.UserName).Success)
                    {
                        ModelState.AddModelError("", "User name must not contain special characters, only letters and digits are allowed.");
                        return View(model);
                    }
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.name);

                    new BLLAspNetUser().SetActiveDirectoryUser(user.Id, model.is_active_directory_user);
                    dbBranchUser.Create(new tblBranchUser() { branch_id = branch_id, user_id = user.Id });

                    return RedirectToAction("Index", "BranchUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                ModelState.AddModelError("", "An error occurred while registering the user.");
            }

            //ViewBag.branch_id = dbBranch.GetAllBranch();
            ViewBag.branchList = dbBranch.GetAllBranch();
            ViewBag.name = new SelectList(dbRoles.GetAllRoles(), "name", "name", model.name);
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType(model.is_active_directory_user.ToString());
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private bool CheckActiveDirectory(string userName)
        {
            string adDomainName = ApplicationSetting.ActiveDirectoryInfo;
            try
            {
                //using (DirectoryEntry entry = new DirectoryEntry($"LDAP://{adDomainName}"))
                //{
                //    using (DirectorySearcher directorySearcher = new DirectorySearcher(entry))
                //    {
                //        directorySearcher.Filter = "(sAMAccountName=" + userName + ")";
                //        SearchResult searchResult = directorySearcher.FindOne();
                //        if (searchResult != null)
                //        {
                //            // The user was found in Active Directory
                //            return true;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in CheckActiveDirectory: {ex.Message}");
            }

            // User was not found in Active Directory or an error occurred
            return false;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, model.RememberBrowser);

            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            else if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        [AuthorizationFilter(Roles = "Admin")]
        public ActionResult AdminRegister()
        {
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType();
            return View();
        }


        [HttpPost]
        [AuthorizationFilter(Roles = "Admin")]
        public async Task<ActionResult> AdminRegister(AdminRegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Hometown = model.Hometown, PhoneNumber = model.Mobile };

            try
            {
                if (model.is_active_directory_user > 0)
                {
                    if (!CheckActiveDirectory(model.UserName))
                    {

                        ModelState.AddModelError("", "User not found in Active Directory.");
                    }
                    else
                    {
                        model.ConfirmPassword = model.Password = "Ssl@1234";
                        ModelState.Clear();
                    }
                }
                else
                {
                    if (model.Password.ToUpper().Contains(model.UserName.ToUpper()))
                    {
                        ModelState.AddModelError("", "Password must have not contained User Name.");
                    }
                    Regex regex = new Regex(@"^[a-zA-Z0-9.\s]+$");
                    if (!regex.Match(model.UserName).Success)
                    {
                        ModelState.AddModelError("", "User name must not have special character, only letter and digit allow.");
                    }
                }

                if (TryValidateModel(model))
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, model.is_active_directory_user);
                        await _userManager.AddToRoleAsync(user, "Admin");

                        return RedirectToAction("Index", "BranchUsers");
                    }
                    AddErrors(result);
                }
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType(model.is_active_directory_user.ToString());
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
            }
        }


        [AuthorizationFilter(Roles = "Admin,Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800708)]
        public ActionResult EditUser(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }
            string user_id = Cryptography.Decrypt(userId, true);

            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            var branchUser = new BLLBranchUsers().GetAll().Where(x => x.user_id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            string userInRole = new BLLAspNetUserRoles().GetRolesByUserId(user.Id).Select(x => x.Name).FirstOrDefault();
            string isAdmin = new BLLAspNetUserRoles().GetRolesByUserId(new SessionManager(_session).user_id).Select(x => x.Name).FirstOrDefault();
            EditUserViewModel model = new EditUserViewModel()
            {
                UserName = user.UserName,
                Mobile = user.PhoneNumber,
                Email = user.Email,
                Hometown = user.Hometown,
                userBranchId = branchUser.user_branch_id,
                transfer_by = new SessionManager(_session).user_id,
                user_role = userInRole,
                user_id = user.Id
            };
            //var y = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", branchUser.branch_id);
            if (isAdmin == "Admin")
            {
                //ViewBag.branch_id = new SelectList(dbBranch.GetAllBranch(), "branch_id", "branch_name", branchUser.branch_id);
                ViewBag.branch_id = dbBranch.GetAllBranch();
            }
            else
            {
                //ViewBag.branch_id = new SelectList(dbBranch.GetBranchesByUserId(new SessionManager(_session).user_id), "branch_id", "branch_name", branchUser.branch_id);
                ViewBag.branch_id = dbBranch.GetBranchesByUserId(new SessionManager(_session).user_id);
            }
            return View(model);
        }

        // POST: /Account/AdminRegister
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin,Branch Admin")]
        public async Task<ActionResult> EditUser(EditUserViewModel model, int userBranchId, string transfer_by)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("Error");
            }

            //user.Hometown = model.Hometown;
            user.Email = model.Email;
            user.PhoneNumber = model.Mobile;

            dbBranchUser.Edit(new tblBranchUser()
            {
                user_branch_id = userBranchId,
                branch_id = model.branch_id,
                transfer_by = transfer_by,
                user_id = model.user_id
            });

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "BranchUsers");
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }
            }

            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        [RightPrivilegeFilter(PageIds = 800725)]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }

            string user_id = Cryptography.Decrypt(userId, true);

            var userData = await _userManager.FindByIdAsync(user_id);

            var Code = _userManager.GeneratePasswordResetTokenAsync(userData);

            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                UserName = user.UserName,
                Code = Code.Result
            };
            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return View("Error");
            }
            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            var results = await _customUserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (results.Succeeded)
            {
                new BLLAspNetUser().UserForceChangeConfirmed(user.Id, false);
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(results);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/SetDirectoryUser
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<IActionResult> SetDirectoryUser(string userId, int is_active_directory_user)
        {
            if (userId == null)
            {
                return View("Error");
            }
            string user_id = Cryptography.Decrypt(userId, true);

            var userData = await _userManager.FindByIdAsync(user_id);

            var Code = _userManager.GeneratePasswordResetTokenAsync(userData);

            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            ActiveDirectoryTransferViewModel model = new ActiveDirectoryTransferViewModel()
            {
                UserName = user.UserName,
                Code = Code.Result,
                is_active_directory_user = is_active_directory_user,
                Password = (is_active_directory_user > 0 ? "Ssl@1234" : ""),
                ConfirmPassword = (is_active_directory_user > 0 ? "Ssl@1234" : "")
            };
            return View(model);
        }

        // POST: /Account/SetDirectoryUser
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public async Task<ActionResult> SetDirectoryUser(ActiveDirectoryTransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return View("Error");
            }
            if (model.is_active_directory_user > 0)
            {
                if (!CheckActiveDirectory(user.UserName))
                {

                    ModelState.AddModelError("", "User not found in Active Directory.");
                    return View(model);
                }
                else
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, 1);
                        return RedirectToAction("SetDirectoryUserConfirmation", "Account", new { isTransferToActiveDirectory = true });
                    }
                    AddErrors(result);
                }
            }
            else
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    new BLLAspNetUser().UserForceChangeConfirmed(user.Id, false);
                    new BLLAspNetUser().SetActiveDirectoryUser(user.Id, 0);
                    return RedirectToAction("SetDirectoryUserConfirmation", "Account", new { isTransferToActiveDirectory = false });
                }
                AddErrors(result);
            }

            return View();
        }

        // GET: /Account/SetDirectoryUserConfirmation
        [AuthorizationFilter(Roles = "Admin, Branch Admin")]
        public ActionResult SetDirectoryUserConfirmation(bool isTransferToActiveDirectory)
        {
            ViewBag.isTransferToActiveDirectory = isTransferToActiveDirectory;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            try
            {
                var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                return Challenge(properties, provider);

            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userdata = await _userManager.FindByLoginAsync(model.SelectedProvider, "sshs");

            var token = await _userManager.GenerateTwoFactorTokenAsync(userdata, model.SelectedProvider);

            if (String.IsNullOrEmpty(token))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                return RedirectToLocal(returnUrl);
            }
            else if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            }
            else if (result.IsNotAllowed)
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Hometown = model.Hometown };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SessionOut()
        {
            notifyDisplay _notifyDisplay = new notifyDisplay(_hubContext);
            try
            {
                await _signInManager.SignOutAsync();

                SessionManager sm = new SessionManager(_session);
                if (sm.LoginProvider.Length > 0)
                {
                    if (sm.counter_id > 0)
                    {
                       await _notifyDisplay.SendMessages(sm.branch_id, sm.counter_no, "", false, true, false, false);
                        await _notifyDisplay.CounterStatusChanged(sm.branch_id);
                    }
                    dbUser.UpdateLogoutInfo(sm.LoginProvider, null, "Session time out");
                }
                sm.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        // GET: /Account/LogOff
        [HttpGet]
        //[Authorize]
        public ActionResult LogOff()
        {
            SessionManager sm = new SessionManager(_session);
            if (sm.LoginProvider.Length > 0)
            {
                dbUser.UpdateUserSetIdle(sm.LoginProvider);
            }

            ViewBag.logout_type_id = new SelectList(new BLLLogoutType().GetAll().Where(w => w.is_active == 1), "logout_type_id", "logout_type_name");
            return View();
        }

        // POST: /Account/LogOff
        [HttpPost]
        public async Task<ActionResult> LogOff(int? logout_type_id)
        {
            notifyDisplay _notifyDisplay = new notifyDisplay(_hubContext);
            await _signInManager.SignOutAsync();

            SessionManager sm = new SessionManager(_session);
            if (sm.LoginProvider.Length > 0)
            {
                if (sm.counter_id > 0)
                {
                    await _notifyDisplay.SendMessages(sm.branch_id, sm.counter_no, "", false, true, false, false);
                    await _notifyDisplay.CounterStatusChanged(sm.branch_id);
                }
                if (logout_type_id.HasValue)
                    dbUser.UpdateLogoutInfo(sm.LoginProvider, logout_type_id.Value, " ");
                else
                    dbUser.UpdateLogoutInfo(sm.LoginProvider, null, " ");
            }
            sm.Close();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult OuterLogOff([FromBody] OuterLogOffModel model)
        {
            string responseJson = String.Empty;
            OuterLogoutResponseModel outerLogout = new OuterLogoutResponseModel();
            try
            {
                ApiManager.ValidUserBySecurityToken(model.securityToken);
                string loginProvider = Cryptography.Decrypt(model.securityToken, true);
                dbUser.UpdateLogoutInfo(loginProvider, null, "External Use only");

                outerLogout = new OuterLogoutResponseModel()
                {
                    success = true,
                    message = "Log out success"
                };
                return Ok(outerLogout);
            }
            catch (Exception ex)
            {
                outerLogout = new OuterLogoutResponseModel()
                {
                    success = true,
                    message = ex.Message
                };
                return Ok(outerLogout);               
            }
            finally
            {
                string requestJson = JsonConvert.SerializeObject(model.securityToken);
                ApiManager.Loggin(model.securityToken, "OuterLogOff", requestJson, responseJson);
            }
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
    }
}
