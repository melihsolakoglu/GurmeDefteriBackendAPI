using GurmeDefteriWebUI.Models.Dto;

namespace GurmeDefteriWebUI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://20.81.205.102:5000/api/Admin/")
            };        
        }
        public async Task<List<User>> GetPagedUserAsync(int page, int pageSize)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"GetAllUsersPageByPage?page={page}&pageSize={pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var responseUsers = await response.Content.ReadAsAsync<List<User>>();
                    return responseUsers;
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
        public async Task<List<User>> GetPagedUserWithKeyAsync(int page, int pageSize, string key)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"SearchUserByNameWithPagination?page={page}&pageSize={pageSize}&userName={key}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<User>>();
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

        public async Task<string> GetPageCountUserAsync(int pageSize)
        {

            string apiUrl = $"GetPageCountUser?pageSize={pageSize}";

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

        public async Task<string> GetPageCountUseryNameAsync(int pageSize, string foodname)
        {

            string apiUrl = $"GetPageCountUserByName?pageSize={pageSize}&name={foodname}";

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

        public async Task<User> GetUserByMailAsync(string mail)
        {
            string requestUri = $"GetUserByMail?userMail={mail}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<User>();
                return content;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> AddUser(User userModel)
        {
            var requestData = new
            {
                userModel.Name,
                userModel.Age,
                userModel.Email,
                userModel.Password,
                userModel.Role
            };
            var response = await _httpClient.PostAsJsonAsync("AddUser", requestData);
            if (response.IsSuccessStatusCode)
            {
             
                return  "Ok";
            }
            else
            {
                return "Error";
            }
        }

        public async Task<string> UpdateUser(User userModel)
        {
            var requestData = new
            { 
                id=userModel.Id,
                name=userModel.Name,
                age=userModel.Age,
                email=userModel.Email,
                password=userModel.Password,
                role=userModel.Role


            };
            var response = await _httpClient.PutAsJsonAsync("UpdateUser", requestData);
            if (response.IsSuccessStatusCode)
            {

                return "Ok";
            }
            else
            {
                return "Error";
            }
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            string endpoint = "DeleteUser";
            try
            {
                // HTTP DELETE isteği gönder
                HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint + "?userId=" + userId);

                // Yanıtın durumunu kontrol et
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Kullanıcı başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Kullanıcı silinirken bir hata oluştu. Durum kodu: " + response.StatusCode);
                }
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return ex.Message;
            }
        }
    }
}
