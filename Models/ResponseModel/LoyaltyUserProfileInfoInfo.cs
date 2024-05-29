using Newtonsoft.Json;

namespace SQMS.Models.ResponseModel
{
    public class LoyaltyProfileInfo
    {
        [JsonProperty("availablePoints")]
        public int AvailablePoints { get; set; }

        [JsonProperty("currentTierLevel")]
        public string CurrentTierLevel { get; set; }

        [JsonProperty("expiryDate")]
        public string ExpiryDate { get; set; }

        [JsonProperty("pointsExpiring")]
        public int PointsExpiring { get; set; }

        [JsonProperty("enrolledDate")]
        public string EnrolledDate { get; set; }

        [JsonProperty("enrolledChannel")]
        public string EnrolledChannel { get; set; }
    }

    public class LoyaltyResponse
    {
        [JsonProperty("loyaltyProfileInfo")]
        public LoyaltyProfileInfo LoyaltyProfileInfo { get; set; }

        [JsonProperty("msisdn")]
        public long Msisdn { get; set; }

        [JsonProperty("responseDateTime")]
        public string ResponseDateTime { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("statusMsg")]
        public string StatusMsg { get; set; }

        [JsonProperty("transactionID")]
        public string TransactionID { get; set; }
    }
}
