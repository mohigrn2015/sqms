using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace SQMS.Helper
{
    public class CustomPasswordHasher:IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            // Implement the hashing algorithm used in your .NET Framework application
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            // Implement the verification logic based on your .NET Framework application's hashing algorithm
            var hashedProvidedPassword = HashPassword(user, providedPassword);
            if (hashedPassword == hashedProvidedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}
