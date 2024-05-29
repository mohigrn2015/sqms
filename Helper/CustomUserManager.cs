using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Common;
using SQMS.BLL;
using SQMS.Models;

namespace SQMS.Helper
{
    public class CustomUserManager<TUser> : UserManager<TUser> where TUser : class
    {
        public CustomUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();
            var userData = user;

            IdentityUser identityUser = userData as IdentityUser;

            try
            {
                string? passwordHash = await UpdatePasswordHashV2(user, newPassword, validatePassword: true);
                
                ChangePassReqModel changePassReqModel = new ChangePassReqModel();
                changePassReqModel.userId = identityUser.Id;
                changePassReqModel.currPass = identityUser.PasswordHash;
                changePassReqModel.newPass = passwordHash;
                changePassReqModel.userName = identityUser.UserName;
                
                var result = await UpdatePasswordHash(user, newPassword, validatePassword: true).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    return result;
                }                

                string response = new WebLoginBLL().Change_Pass(changePassReqModel);

                if (!String.IsNullOrEmpty(response) && response == identityUser.Id)
                {
                    result = IdentityResult.Success;
                    return await UpdateUserAsync(user).ConfigureAwait(false);
                }
                else
                {
                    return IdentityResult.Failed(ErrorDescriber.PasswordMismatch());
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }


        public override async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            CustomPasswordHasher customPasswordHasher = new CustomPasswordHasher();
            ThrowIfDisposed();

            if (!await VerifyUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token).ConfigureAwait(false))
            {
                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
            }

            var userData = user;

            IdentityUser identityUser = userData as IdentityUser;

            ApplicationUser applicationUser = userData as ApplicationUser;

            //// Hash the new password
            //var passwordHash = customPasswordHasher.HashPassword(applicationUser, newPassword); // Implement this method to hash the password
            string? passwordHash = await UpdatePasswordHashV2(user, newPassword, validatePassword: true); // Implement this method to hash the password

            var result = await UpdatePasswordHash(user, passwordHash, validatePassword: true).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return result;
            }

            identityUser.PasswordHash = passwordHash;

            try
            {
                try
                {
                    new WebLoginBLL().Update_Pas(identityUser);
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

            return await UpdateUserAsync(user).ConfigureAwait(false);
        }

        public async Task<string?> UpdatePasswordHashV2(TUser user, string? newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword).ConfigureAwait(false);
            }

            var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;


            return hash;
        }

        //protected override async Task<IdentityResult> UpdateUserAsync(TUser user)
        //{
        //    var result = await base.UpdateUserAsync(user).ConfigureAwait(false);

        //    return result;
        //}
    }


}
