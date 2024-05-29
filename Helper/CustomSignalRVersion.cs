namespace SQMS.Helper
{
    public static class CustomSignalRVersion
    {
        public static string DetermineClientSignalRVersion(HttpContext context)
        {
            return "1.0";
        }
    }
}
