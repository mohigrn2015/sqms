using SQMS.Utility;

namespace SQMS.Helper
{
    public class SignalRServiceOld : ISignalRService
    {
        public void Configure(HttpContext context)
        {
            context.Response.Headers.Add("SignalR-Protocol", "messagepack");
        }
    }
}
