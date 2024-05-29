using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using SQMS.BLL;
using SQMS.Models;
using System.Threading;
using System.Threading.Tasks;


public class CustomUserStoreV2 : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>, IUserRoleStore<IdentityUser>
{
    private readonly List<IdentityUser> _users = new List<IdentityUser>();
    private readonly List<IdentityUserRole<string>> _userRoles = new List<IdentityUserRole<string>>();
    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
         return Task.FromResult(user.Id);
    }

    public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    // Implement other IUserStore<IdentityUser> methods...

    public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }


    public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
    {

        user.PasswordHash = passwordHash;
        // Implement setting password hash for the user
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        //var passwordHash = "ADTcDsxhYqkYVZUAlhm7qAZy9bQ/i8UraosWamQVT4aidzuZiD1kCIgXrgJyI4Uovw==";
        //SQMS.Models.UserInfoV2 userInfo = new WebLoginBLL().GetUserInformation(user.UserName.ToString());
        var passwordHash = user.PasswordHash;// userInfo.password_hash;

        return Task.FromResult(passwordHash);
    }

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        // Implement checking if the user has a password
        return Task.FromResult(false); // Replace false with actual logic
    }
    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ApplicationUser applicationUser = user as ApplicationUser;
        
        string userInfo = new WebLoginBLL().Insert_user(user, applicationUser.Hometown,0,0);

        if (!String.IsNullOrEmpty(userInfo))
        {
            _users.Add(user);

            return Task.FromResult(IdentityResult.Success);
        }
        else
        {
            throw new InvalidOperationException();
        }        
    }
    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        _users.Remove(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public void Dispose()
    {
    }
    public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        IdentityUser identityUser = null;
        SQMS.Models.UserInfoV2 userInfo = new WebLoginBLL().GetUserInformationById(userId);
        if(userInfo == null) { return Task.FromResult(identityUser); }
        var user = new IdentityUser
        {
            Id = userInfo.user_id,
            UserName = userInfo.user_login_name,
            NormalizedUserName = userInfo.user_name,
            PasswordHash = userInfo.password_hash,
            Email = userInfo.email
        };
        return Task.FromResult(user);
    }

    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        normalizedUserName = normalizedUserName.Trim();
        normalizedUserName = normalizedUserName.ToLower();
        
        SQMS.Models.UserInfoV2 userInfo = new WebLoginBLL().GetUserInformation(normalizedUserName);

        if (userInfo == null) 
            return null;

        var user = new IdentityUser
        {
            Id = userInfo.user_id,
            UserName = userInfo.user_login_name,
            NormalizedUserName = userInfo.user_name,
            PasswordHash = userInfo.password_hash,
            Email = userInfo.email           
        };
        return user;
    }
    
    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        //try
        //{
        //    new WebLoginBLL().Update_Pas(user);
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}

        return Task.FromResult(IdentityResult.Success);
    }

    public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {       
        
        var roleId = new WebLoginBLL().GetRoleId(roleName); 

        _userRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = roleId });

        new WebLoginBLL().Insert_Role(user.Id,roleId);

        return Task.CompletedTask;
    }

    public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        // Remove the user from the specified role
        var userRole = _userRoles.FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == roleName);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
        }
        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        SQMS.Models.UserInfoV2 userInfo = new WebLoginBLL().GetUserInformation(user.UserName);
        if(userInfo != null)
        {
            var roles = _userRoles.Where(ur => ur.UserId == user.Id).Select(ur => userInfo.role_name).ToList();
            List<string> result = new List<string>(userInfo.role_name.Split(','));
            return Task.FromResult<IList<string>>(result);
        }
        return Task.FromResult<IList<string>>(null);
        // Get the roles associated with the user
    }

    public Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        var isInRole = _userRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == roleName);
        return Task.FromResult(isInRole);
    }

    public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword, CancellationToken cancellationToken)
    {
        var result = new IdentityResult();

        user.PasswordHash = newPassword;

        try
        {
            try
            {
                new WebLoginBLL().Update_Pas(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            result = IdentityResult.Success;
        }
        catch (Exception ex)
        {
            result = IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }

        return result;
    }
}
public class CustomIdentityUser : IdentityUser
{
    public bool Succeeded { get; set; }
    public bool IsLockedOut { get; set; }
}
