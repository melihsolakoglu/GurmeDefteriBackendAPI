using GurmeDefteriWebUI.Helpers;
using GurmeDefteriWebUI.Models.Dto;
using System.Net.Http.Headers;

namespace GurmeDefteriWebUI.Services
{
    public class ScoredFoodService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ScoredFoodService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.ApiUrl + "Admin/")
            };

            _httpContextAccessor = new HttpContextAccessor();
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["JwtCookie"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        }

        public async Task<List<string>> GetAllUserMails()
        {

            string apiUrl = $"GetAllUserEmails";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    List<string> responseBody = await response.Content.ReadAsAsync<List<string>>();
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
        public async Task<List<string>> GetAllFoodsNames()
        {

            string apiUrl = $"GetAllFoodsNames";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    List<string> responseBody = await response.Content.ReadAsAsync<List<string>>();
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
        public async Task<List<ScoredFood>> GetPagedScoredFoodAsync(int page, int pageSize)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"ShowAdminScoredFoods?pageNumber={page}&pageSize={pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var responseSCoredFoods = await response.Content.ReadAsAsync<List<ScoredFood>>();
                    return responseSCoredFoods;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetPageCountScoredFoodAsync(int pageSize)
        {

            string apiUrl = $"GetPageCountScoredFood?pageSize={pageSize}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    return "API çağrısı başarısız oldu. Durum kodu: " + response.StatusCode;
                }
            }
            catch (HttpRequestException e)
            {
                return "API çağrısında hata oluştu: " + e.Message;
            }
        }
        public async Task<List<ScoredFood>> GetPagedScoredFoodWithKeyAsync(int pageNumber, int pageSize, string searchTerm)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"SearchScoredFoods?pageNumber={pageNumber}&pageSize={pageSize}&searchTerm={searchTerm}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<ScoredFood>>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> GetPageCountScoredFoodWithKeyAsync(int pageSize, string name)
        {

            string apiUrl = $"GetPageCountScoredFoodsByName?pageSize={pageSize}&name={name}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    return "API çağrısı başarısız oldu. Durum kodu: " + response.StatusCode;
                }
            }
            catch (HttpRequestException e)
            {
                return "API çağrısında hata oluştu: " + e.Message;
            }
        }
        public async Task<string> AddScoredFood(string userEmail, string foodName, int score)
        {
            var requestData = new
            {
                userEmail,
                foodName,
                score
            };
            var response = await _httpClient.PostAsJsonAsync("AddAdminScoredFoods", requestData);
            if (response.IsSuccessStatusCode)
            {

                return "Ok";
            }
            else
            {
                return "Error";
            }
        }
        public async Task<string> DeleteScoredFoodAsync(string scoredFoodId)
        {
            string endpoint = "DeleteScoredFoods";
            try
            {
                // HTTP DELETE isteği gönder
                HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint + "?id=" + scoredFoodId);

                // Yanıtın durumunu kontrol et
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Yemek Skoru başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Yemek Skoru silinirken bir hata oluştu. Durum kodu: " + response.StatusCode);
                }
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return ex.Message;
            }
        }

        public async Task<bool> CheckScoredFood(string userEmail, string foodName)
        {
            string endpoint = "CheckScoredFood";
            HttpResponseMessage response = await _httpClient.GetAsync($"{endpoint}?userEmail={userEmail}&foodName={foodName}");
            bool isSCored = await response.Content.ReadAsStringAsync() == "true";
            return isSCored;

        }

        public async Task<ScoredFood> GetScoredFoodUByIdAsync(string scoredID)
        {
            string endpoint = "GetScoredFoodWithId";
            HttpResponseMessage response = await _httpClient.GetAsync($"{endpoint}?scoredFoodId={scoredID}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<ScoredFood>();
                return content;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> UpdateScoredFood(string id,int score)
        {
            string endpoint = "UpdateScoredFoods";
            var requestUri = $"{endpoint}?id={id}&score={score}";


            var content = new StringContent(string.Empty);

            HttpResponseMessage response = await _httpClient.PutAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
                return "Ok";
            else
                return "Error";


        }
    }
}
