using GurmeDefteriBackEndAPI.Models.Dto;
using GurmeDefteriBackEndAPI.Models.ViewModel;
using GurmeDefteriBackEndAPI.Models;
using GurmeDefteriBackEndAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GurmeDefteriBackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }


        [HttpGet("GetAllFoods")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> GetAllFoodsAsync()
        {
            var foods = _userService.GetFoods();

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

        [HttpGet("GetFoodByName")]
        public async Task<ActionResult<FoodItemWithImageBytes>> GetFoodByNameAsync(string name)
        {
            var foodItem = _userService.GetFoods().FirstOrDefault(f => f.Name == name);

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

        [HttpGet("FoodSearch")]
        public async Task<ActionResult<List<FoodItemWithImageBytes>>> FoodSearchAsync(string query, int page = 1, int pageSize = 30)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page or page size cannot be lower than one.");
            }
            var foods = _userService.GetFoods().Where(f => f.Name.Contains(query));

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
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(UserAPI updatedUser)
        {
            try
            {
                _userService.UpdateUser(updatedUser);
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
                _userService.DeleteUser(userId);
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
                _userService.AddUser(newUser);
                return Ok("User added successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetUserById")]
        public ActionResult<User> GetUserById(string userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound("No user found with given ID."); // Kullanıcı bulunamadı durumunu işler
            }
            var userAPI = new UserAPI
            {
                Name = user.Name,
                Password = user.Password,
                Age = user.Age,
                Id = user.Id.ToString(),
                Email = user.Email,
                Role = user.Role
            };
            return Ok(userAPI);
        }

        [HttpGet("GetUserByMail")]
        public ActionResult<User> GetUserByMail(string userMail)
        {
            var user = _userService.GetUserByMail(userMail);

            if (user == null)
            {
                return NotFound("No user found with given ID."); // Kullanıcı bulunamadı durumunu işler
            }
            var userAPI = new UserAPI
            {
                Name = user.Name,
                Password = user.Password,
                Age = user.Age,
                Id = user.Id.ToString(),
                Email = user.Email,
                Role = user.Role
            };
            return Ok(userAPI);
        }
    }
}
