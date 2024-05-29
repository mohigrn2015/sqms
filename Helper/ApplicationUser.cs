using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

public class ApplicationUser : IdentityUser
{
    public string Hometown { get; set; }
    public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
        var userIdentity = await manager.CreateAsync(this, "ApplicationCookie");

        return userIdentity;
    }
}
