
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
        [HttpGet("GetAllUser")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _adminService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("GetAllFoods")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetAllFoodsAsync()
        {
            var foods = _adminService.GetFoods();

            var tasks = foods.Select(async foodItem =>
            {

                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                    Id = foodItem.Id.ToString()
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }
        [HttpGet("GetFoodsWithPagebyPage")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetFoodsAsync(int page , int pageSize)
        {
            var foods = _adminService.GetFoods();

            var pagedFoods = foods.Skip((page - 1) * pageSize).Take(pageSize);

            var tasks = pagedFoods.Select(async foodItem =>
            {
                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                    Id = foodItem.Id.ToString()
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }
        [HttpGet("GetFoodByName")]
        public async Task<ActionResult<FoodItemWithImageBytes>> GetFoodByNameAsync(string name)
        {
            var foodItem = _adminService.GetFoods().FirstOrDefault(f => f.Name == name);

            if (foodItem == null)
            {
                return NotFound();
            }

            var foodItemWithImage = new FoodItemWithImageBytes
            {
                Name = foodItem.Name,
                Country = foodItem.Country,
                ImageBytes = foodItem.Image,
                Id = foodItem.Id.ToString()
            };

            return Ok(foodItemWithImage);
        }
        [HttpGet("GetFoodByNameWithPagination")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetFoodByNameWithPaginationAsync(string foodName, int page = 1, int pageSize = 30)
        {
            var foods = _adminService.GetFoods().Where(f => f.Name.ToUpper().Contains(foodName.ToUpper()));

            var pagedFoods = foods.Skip((page - 1) * pageSize).Take(pageSize);

            var tasks = pagedFoods.Select(async foodItem =>
            {
              

                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                     Id = foodItem.Id.ToString()
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }
        [HttpGet("GetPageCountFood")]
        public  int GetPageCountFood( int pageSize = 30)
        {
            int totalFoodCount = _adminService.GetFoodCount();
            int pageCount = totalFoodCount / pageSize;
            pageCount += (totalFoodCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }

        [HttpGet("GetPageCountFoodByName")]
        public int GetPageCountFoodByName(int pageSize = 30,string name="")
        {
            int totalFoodCount = _adminService.GetFoodCountByName(name);

            int pageCount = totalFoodCount / pageSize;
            pageCount += (totalFoodCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }
        [HttpGet("FoodSearch")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> FoodSearchAsync(string query, int page = 1, int pageSize = 30)
        {
            var foods = _adminService.GetFoods().Where(f => f.Name.Contains(query));

            var pagedFoods = foods.Skip((page - 1) * pageSize).Take(pageSize);

            var tasks = pagedFoods.Select(async foodItem =>
            {
        
                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                    Id = foodItem.Id.ToString()
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }



        [HttpGet("GetAllScoredFoods")]
        public ActionResult<List<ScoredFoods>> GetAllScoredFoods()
        {
            var scoredFoods = _adminService.GetScoredFoods();
            return Ok(scoredFoods);
        }
        [HttpGet("GetUserById/{userId}")]
        public IActionResult GetUserById(string userId)
        {
            var user = _adminService.GetUserWithId(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPut("UpdateUser/{userId}")]
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
        [HttpDelete("DeleteUser/{userId}")]
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
        [HttpPost("AddUser")]
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
        [HttpPost("AddFood")]
        public async Task<IActionResult> AddFoodAsync([FromForm] FoodTemp foodTemp)
        {
            try { 
                    

                _adminService.AddFood(foodTemp.Name, foodTemp.Country,foodTemp.Image);
                return Ok("Food added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteFood")]
        public IActionResult DeleteFood(string foodId)
        {
            try
            {
                _adminService.DeleteFood(foodId);
                return Ok("Food deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateFood/")]
        public IActionResult UpdateFood([FromForm] FoodItemWithImageBytes foodTemp)
        {
            try
            {
                _adminService.UpdateFood(foodTemp);
                return Ok("Food updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        
    }
}
