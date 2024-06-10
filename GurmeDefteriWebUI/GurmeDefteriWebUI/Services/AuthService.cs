using GurmeDefteriWebUI.Helpers;
using System.Text.Json;

namespace GurmeDefteriWebUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.ApiUrl)
            };
        }

        public async Task<string> Authenticate(string Email, string password)
        {
            var requestData = new
            {
                Email,  
                password
            };

            var response = await _httpClient.PostAsJsonAsync("Auth/AdminLogin", requestData);
            string responseBody = await response.Content.ReadAsStringAsync();
            string code = response.StatusCode.ToString();
            if(code=="OK")
            return responseBody;

            return "";
             
        }
    }
}
