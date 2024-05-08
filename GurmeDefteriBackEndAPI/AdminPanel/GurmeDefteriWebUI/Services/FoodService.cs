using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Formatting;
using System.Net.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;


namespace GurmeDefteriWebUI.Services
{
    public class FoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://20.81.205.102:5000/api/Admin/")
            };
        }

        private string ResizeBase4Image(string Base64Image,int width ,int height)
        {
            byte[] imageBytes = Convert.FromBase64String(Base64Image);

            using (var inputStream = new MemoryStream(imageBytes))
            using (var outputStream = new MemoryStream())
            using (var image = Image.Load(inputStream))
            {
                image.Mutate(x => x.Resize(width, height));
                image.Save(outputStream, new JpegEncoder());
                byte[] resizedImageBytes = outputStream.ToArray();
                return Convert.ToBase64String(resizedImageBytes);
            }
        }
        public async Task<string> AddFood(string name, string country, string fileBase64,string category)
        {
            try
            {
                string resizedBase64String = ResizeBase4Image(fileBase64,400, 300);

              
                var content = new MultipartFormDataContent
                {
                    { new StringContent(name), "Name" },
                    { new StringContent(country), "Country" },
                    { new StringContent(resizedBase64String), "Image" },
                    { new StringContent(category), "Category" }
                };

                var response = await _httpClient.PostAsync("AddFood", content);
                response.EnsureSuccessStatusCode();

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
        public async Task<string> UpdateFood(Food foodTemp)
        {
            try
            {
                string resizedBase64String = ResizeBase4Image(foodTemp.ImageBytes, 400, 300);
                var content = new MultipartFormDataContent
                {
                    { new StringContent(foodTemp.Id), "Id" },
                    { new StringContent(foodTemp.Name), "Name" },
                    { new StringContent(foodTemp.Country), "Country" },
                    { new StringContent(resizedBase64String), "ImageBytes" },
                    { new StringContent(foodTemp.Category), "Category" }
                };


                var response = await _httpClient.PutAsync("UpdateFood", content);
                response.EnsureSuccessStatusCode();

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<List<Food>> GetPagedFoodAsync(int page, int pageSize)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"GetFoodsWithPagebyPage?page={page}&pageSize={pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var responseFoods= await response.Content.ReadAsAsync<List<Food>>();
                    return responseFoods;
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
        public async Task<List<Food>> GetPagedFoodWithKeyAsync(int page, int pageSize,string key)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"GetFoodByNameWithPagination?page={page}&pageSize={pageSize}&foodName={key}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<Food>>();
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

        public async Task<Food> GetFoodByNameAsync(string name)
        {
            string requestUri = $"GetFoodByName?name={name}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            // 2. Gelen yanıtı işleyin ve yemek öğesini alın
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<Food>();
                return content;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> DeleteFoodAsync(string foodId)
        {
            string endpoint = "DeleteFood";
            try
            {
                // HTTP DELETE isteği gönder
                HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint + "?foodId=" + foodId);

                // Yanıtın durumunu kontrol et
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Yiyecek başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Yiyecek silinirken bir hata oluştu. Durum kodu: " + response.StatusCode);
                }
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> GetPageCountFoodAsync(int pageSize)
        {

            string apiUrl = $"GetPageCountFood?pageSize={pageSize}";

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

        public async Task<string> GetPageCountFoodByNameAsync(int pageSize,string foodname)
        {

            string apiUrl = $"GetPageCountFoodByName?pageSize={pageSize}&name={foodname}";

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





