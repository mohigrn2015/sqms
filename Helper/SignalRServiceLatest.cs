using SQMS.Utility;

namespace SQMS.Helper
{
    public class SignalRServiceLatest : ISignalRService
    {
        public void Configure(HttpContext context)
        {
            context.Response.Headers.Add("SignalR-Protocol", "json");
        }
    }    
}
