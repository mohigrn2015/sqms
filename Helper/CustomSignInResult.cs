using Microsoft.AspNetCore.Identity;

namespace SQMS.Helper
{
    public class CustomSignInResult : SignInResult
    {
        public CustomSignInResult(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor)            
        {
            IsLockedOut = isLockedOut;
            IsNotAllowed = isNotAllowed;
            RequiresTwoFactor = requiresTwoFactor;
            Succeeded = succeeded;
            
        }
    }

}
