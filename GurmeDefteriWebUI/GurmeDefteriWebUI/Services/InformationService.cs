using GurmeDefteriWebUI.Helpers;
using System.Net.Http.Headers;

namespace GurmeDefteriWebUI.Services
{
    public class InformationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InformationService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.ApiUrl + "Admin/")
            };
            _httpContextAccessor = new HttpContextAccessor();
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["JwtCookie"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }


        public async Task<string> GetLogsWithDate(string date)
        {

            string apiUrl = $"GetLog?date={date}";

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
    }
}
