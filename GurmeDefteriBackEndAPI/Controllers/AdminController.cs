
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
                    Id = foodItem.Id.ToString(),
                    Category = foodItem.Category
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }

        [HttpGet("GetFoodsWithPagebyPage")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetFoodsAsync(int page = 1, int pageSize = 30)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page or page size cannot be lower than one.");
            }
            var foods = _adminService.GetFoods();

            var pagedFoods = foods.Skip((page - 1) * pageSize).Take(pageSize);

            var tasks = pagedFoods.Select(async foodItem =>
            {
                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                    Id = foodItem.Id.ToString(),
                    Category = foodItem.Category
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
                return NotFound("No food found with this query.");
            }

            var foodItemWithImage = new FoodItemWithImageBytes
            {
                Name = foodItem.Name,
                Country = foodItem.Country,
                ImageBytes = foodItem.Image,
                Id = foodItem.Id.ToString(),
                Category = foodItem.Category
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
                    Id = foodItem.Id.ToString(),
                    Category = foodItem.Category
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }

        [HttpGet("GetPageCountFood")]
        public int GetPageCountFood(int pageSize = 30)
        {
            int totalFoodCount = _adminService.GetFoodCount();
            int pageCount = totalFoodCount / pageSize;
            pageCount += (totalFoodCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }

        [HttpGet("GetPageCountFoodByName")]
        public int GetPageCountFoodByName(int pageSize = 30, string name = "")
        {
            int totalFoodCount = _adminService.GetFoodCountByName(name);

            int pageCount = totalFoodCount / pageSize;
            pageCount += (totalFoodCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }

        [HttpGet("FoodSearch")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> FoodSearchAsync(string query, int page = 1, int pageSize = 30)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page or page size cannot be lower than one.");
            }
            var foods = _adminService.GetFoods().Where(f => f.Name.Contains(query));

            var pagedFoods = foods.Skip((page - 1) * pageSize).Take(pageSize);

            var tasks = pagedFoods.Select(async foodItem =>
            {

                return new FoodItemWithImageBytes
                {
                    Name = foodItem.Name,
                    Country = foodItem.Country,
                    ImageBytes = foodItem.Image,
                    Id = foodItem.Id.ToString(),
                    Category = foodItem.Category
                };
            });

            var foodListWithImages = await Task.WhenAll(tasks);

            return Ok(foodListWithImages.ToList());
        }

        [HttpPost("AddFood")]
        public async Task<IActionResult> AddFoodAsync([FromForm] FoodTemp foodTemp)
        {
            try
            {
                _adminService.AddFood(foodTemp.Name, foodTemp.Country, foodTemp.Image, foodTemp.Category);
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
        [HttpPut("UpdateFood")]
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



        [HttpGet("GetAllScoredFoods")]
        public ActionResult<List<ScoredFoods>> GetAllScoredFoods()
        {
            var scoredFoods = _adminService.GetScoredFoods();
            return Ok(scoredFoods);
        }
        [HttpPut("UpdateUser")]
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
        [HttpDelete("DeleteUser")]
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

        [HttpGet("GetAllUser")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _adminService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("GetAllUsersPageByPage")]
        public ActionResult<List<User>> GetAllUsersPageByPage(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page or page size cannot be lower than one.");
            }


            var users = _adminService.GetAllUsers();

            var paginatedUsers = users.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            return Ok(paginatedUsers);
        }
        [HttpGet("GetUserById")]
        public ActionResult<User> GetUserById(string userId)
        {
            var user = _adminService.GetUserById(userId);

            if (user == null)
            {
                return NotFound("No user found with given ID."); // Kullanıcı bulunamadı durumunu işler
            }

            return Ok(user);
        }
        [HttpGet("SearchUser")]
        public ActionResult<List<User>> SearchUser(string query, int page = 1, int pageSize = 30)
        {
            
            if (page < 1)
            {
                return BadRequest("Page number cannot be less than 1.");
            }

            
            if (pageSize < 1)
            {
                return BadRequest("Page size cannot be less than 1.");
            }

            var users = _adminService.GetAllUsers()
                .Where(u => u.Name.Contains(query) || u.Email.Contains(query))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(users);
        }

        [HttpGet("SearchUserByNameWithPagination")]
        public async Task<ActionResult<List<User>>> SearchUserByNameWithPaginationAsync(string userName, int page = 1, int pageSize = 30)
        {
            if (userName == null)
            {
                return BadRequest("User name cannot be null.");
            }

            // Sayfa numarasının sıfırın altında olmamasını sağlama
            if (page < 1)
            {
                return BadRequest("Page number cannot be less than 1.");
            }

            // Sayfa boyutunun sıfırın altında olmamasını sağlama
            if (pageSize < 1)
            {
                return BadRequest("Page size cannot be less than 1.");
            }
            var users = _adminService.GetAllUsers()
                .Where(u => u.Name.ToUpper().Contains(userName.ToUpper()))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(users);
        }
        [HttpGet("GetPageCountUser")]
        public int GetPageCountUser(int pageSize = 30)
        {
            int totalUserCount = _adminService.GetUserCount();
            int pageCount = totalUserCount / pageSize;
            pageCount += (totalUserCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }

        [HttpGet("GetPageCountUserByName")]
        public int GetPageCountUserByName(int pageSize = 30, string name = "")
        {
            int totalUserCount = _adminService.GetUserCountByName(name);

            int pageCount = totalUserCount / pageSize;
            pageCount += (totalUserCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }









    }
}
