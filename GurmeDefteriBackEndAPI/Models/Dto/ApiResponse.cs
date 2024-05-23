using Newtonsoft.Json;

namespace GurmeDefteriBackEndAPI.Models.Dto
{
    public class ApiResponse
    {
        [JsonProperty("en_iyi_yemek")]
        public string BestFoodId { get; set; }

        [JsonProperty("puan")]
        public double Score { get; set; }
    }
}
