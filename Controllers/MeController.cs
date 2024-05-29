using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQMS.Models.ViewModels;
using SQMS.SignalRHub;

namespace SQMS.Controllers
{
    [Route("api/Me")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public MeController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public GetViewModel Get()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name);
            return new GetViewModel() { Hometown = ""};// user.Hometown };
        }
    }
}
