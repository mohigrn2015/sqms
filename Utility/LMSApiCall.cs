using Newtonsoft.Json;
using SQMS.Models.RequestModel;
using SQMS.Models.ResponseModel;

namespace SQMS.Utility
{
    public class LMSApiCall
    {
        public async Task<LoyaltyResponse> CallLMSapi(CustomerCategoryReqModel model)
        {
            LoyaltyResponse loyaltyResponse = new LoyaltyResponse();
            try
            {
                var requestBody = new
                {
                    channel = model.channel,
                    msisdn = model.msisdn,
                    transactionID = model.transactionID
                };
                string url = StaticConfigValue.GetLMSApiUrl();

                var jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.banglalink.apihub-v1.0+json");

                    var content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        loyaltyResponse = JsonConvert.DeserializeObject<LoyaltyResponse>(responseContent);

                        return loyaltyResponse;
                    }
                    else
                    {
                        return loyaltyResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
