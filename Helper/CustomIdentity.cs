using System.Security.Principal;

namespace SQMS.Helper
{
    public class CustomIdentity : IIdentity
    {
        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; private set; }
        public string Name { get; }

        public CustomIdentity(string name, bool isAuthenticated, string authenticationType)
        {
            Name = name;
            IsAuthenticated = isAuthenticated;
            AuthenticationType = authenticationType;
        }
    }
}
