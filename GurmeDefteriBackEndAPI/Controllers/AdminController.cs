
using GurmeDefteriBackEndAPI.Models;
using GurmeDefteriBackEndAPI.Models.ViewModel;
using GurmeDefteriBackEndAPI.Services;
using GurmeDefteriWebUI.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace GurmeDefteriBackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController()
        {
            _adminService = new AdminService();
        }
        [HttpGet("users")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _adminService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("foods")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetAllFoodsAsync()
        {
            var foods = _adminService.GetFoods();

            // Yiyecek öğeleri üzerinde paralel işlemler yaparak işlemi hızlandırma
            var tasks = foods.Select(async foodItem =>
            {

                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(foodItem.Image);
                string base64String = Convert.ToBase64String(imageBytes);

                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = base64String
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }
        [HttpGet("scoredfoods")]
        public ActionResult<List<User>> GetAllScoredFoods()
        {
            var scoredFoods = _adminService.GetScoredFoods();
            return Ok(scoredFoods);
        }
        [HttpGet("users/{userId}")]
        public IActionResult GetUserById(string userId)
        {
            var user = _adminService.GetUserWithId(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPut("users/{userId}")]
        public IActionResult UpdateUser(string userId, User updatedUser) 
        {
            try
            {
                _adminService.UpdateUser(userId, updatedUser);
                return Ok("Kullanıcı Başarıyla Güncellendi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                _adminService.DeleteUser(userId);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("users")]
        public IActionResult AddUser(User newUser)
        {
            try
            {
                _adminService.AddUser(newUser);
                return Ok("User added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("foods")]
        public async Task<IActionResult> AddFoodAsync([FromForm] FoodTemp foodTemp)
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "FoodImages");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, foodTemp.Name + ".jpeg");

                using (var image = Image.Load(foodTemp.Image.OpenReadStream()))
                {
                    // Resmi sıkıştırör
                    image.Mutate(x => x.Resize(800, 600)); 

                    // JPEG olarak kaydet
                    image.Save(filePath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 75 }); // Kaliteyi gereksinimlerinize göre ayarlayabilirsiniz
                }

                _adminService.AddFood(foodTemp.Name, foodTemp.Country, Directory.GetCurrentDirectory() + "/FoodImages/" + foodTemp.Name + ".jpeg");
                return Ok("Food added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
