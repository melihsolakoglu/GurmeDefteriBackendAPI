using Newtonsoft.Json;

namespace GurmeDefteriBackEndAPI.Models.Dto
{
    public class FoodApiResponse
    {
        [JsonProperty("puan")]
        public string Score { get; set; }
    }
}
