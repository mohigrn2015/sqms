using Org.BouncyCastle.Asn1.Ocsp;
using SQMS.Models.RequestModel;

namespace SQMS.Utility
{
    public class SMSApiCall
    {
        public async Task<string> SendSms(SMSDataResponseModel model)
        {
            HttpClient _httpClient = new HttpClient();

            string MainUrl = "http://10.10.31.113:13082/cgi-bin/sendsms?username=itapp&password=itapp1&from=9123&to={0}&text={1}&charset=utf-8&coding=2";
            
            string Url = String.Format(MainUrl,model.msisdn,model.message);

            try
            {
                var response = await _httpClient.GetAsync(Url);
                
                if (response.IsSuccessStatusCode)
                {
                    return "SMS sent successfully.";
                }
                else
                {
                    return "Failed to send SMS.";
                }
            }
            catch (HttpRequestException e)
            {
                return e.Message.ToString();
            }
        }
    }
}
