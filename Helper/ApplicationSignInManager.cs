using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQMS.Helper;
using System.Security.Claims;
using System.Threading.Tasks;
namespace SQMS.Helper
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;

        public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _claimsPrincipalFactory = claimsFactory;
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            return await _claimsPrincipalFactory.CreateAsync(user);
        }

        public static ApplicationSignInManager Create(HttpContext context)
        {
            var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var claimsPrincipalFactory = context.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var optionsAccessor = context.RequestServices.GetRequiredService<IOptions<IdentityOptions>>();
            var logger = context.RequestServices.GetRequiredService<ILogger<SignInManager<ApplicationUser>>>();
            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            var confirmation = context.RequestServices.GetRequiredService<IUserConfirmation<ApplicationUser>>();

            return new ApplicationSignInManager(userManager, httpContextAccessor, claimsPrincipalFactory, optionsAccessor, logger, schemes, confirmation);
        }
    }
}