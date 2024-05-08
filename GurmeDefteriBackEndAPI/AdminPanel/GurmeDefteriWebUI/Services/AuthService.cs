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
                BaseAddress = new Uri("http://20.81.205.102:5000/api/")
            };
        }

        public async Task<bool> Authenticate(string Email, string password)
        {
            var requestData = new
            {
                Email,  
                password
            };

            var response = await _httpClient.PostAsJsonAsync("Auth", requestData);
            var responseBody = await response.Content.ReadAsStringAsync();
            var code = response.StatusCode.ToString();
            return code == "OK";
             
        }
    }
}
