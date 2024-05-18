
using GurmeDefteriBackEndAPI.Models;
using GurmeDefteriBackEndAPI.Models.Dto;
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
        public IActionResult UpdateUser(UserAPI updatedUser)
        {
            try
            {
                _adminService.UpdateUser(updatedUser);
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
        public async Task<ActionResult<List<User>>> GetAllUsersAsync()
        {
            var users = _adminService.GetAllUsers();

            var tasks = users.Select(async userItem =>
            {
                return new UserAPI
                {
                    Name = userItem.Name,
                    Password = userItem.Password,
                    Age = userItem.Age,
                    Id = userItem.Id.ToString(),
                    Email = userItem.Email,
                    Role = userItem.Role
                };
            });

            var userList = await Task.WhenAll(tasks);

            return Ok(userList.ToList());
        
    }
        [HttpGet("GetAllUsersPageByPage")]
        public async Task<ActionResult<List<User>>> GetAllUsersPageByPageAsync(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page or page size cannot be lower than one.");
            }


            var users = _adminService.GetAllUsers();

            var paginatedUsers = users.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            var tasks = paginatedUsers.Select(async userItem =>
            {
                return new UserAPI
                {
                    Name = userItem.Name,
                    Password = userItem.Password,
                    Age = userItem.Age,
                    Id = userItem.Id.ToString(),
                    Email = userItem.Email,
                    Role = userItem.Role
                };
            });

            var userList = await Task.WhenAll(tasks);

            return Ok(userList.ToList());
        }
        [HttpGet("GetUserById")]
        public ActionResult<User> GetUserById(string userId)
        {
            var user = _adminService.GetUserById(userId);

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
            var user = _adminService.GetUserByMail(userMail);

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
        [HttpGet("SearchUser")]
        public async Task<ActionResult<List<User>>> SearchUserAsync(string query, int page = 1, int pageSize = 30)
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

            var tasks = users.Select(async userItem =>
            {
                return new UserAPI
                {
                    Name = userItem.Name,
                    Password = userItem.Password,
                    Age = userItem.Age,
                    Id = userItem.Id.ToString(),
                    Email = userItem.Email,
                    Role= userItem.Role
                };
            });

            var userList = await Task.WhenAll(tasks);

            return Ok(userList.ToList());

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

            var tasks = users.Select(async userItem =>
            {
                return new UserAPI
                {
                    Name = userItem.Name,
                    Password = userItem.Password,
                    Age = userItem.Age,
                    Id = userItem.Id.ToString(),
                    Email = userItem.Email,
                    Role = userItem.Role
                };
            });

            var userList = await Task.WhenAll(tasks);

            return Ok(userList.ToList());
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
        [HttpGet("GetPageCountScoredFood")]
        public int GetPageCountScoredFood(int pageSize = 30) 
        {
            int totalScoredFoodCount = _adminService.GetScoredFoodCount();

            int pageCount=totalScoredFoodCount / pageSize;
            pageCount += (totalScoredFoodCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }
        [HttpGet("GetPageCountScoredFoodsByName")]
        public int GetPageCountScoredFoodsByName(int pageSize = 30, string name = "")
        {
            int totalUserCount = _adminService.GetScoredFoodCountByName(name);

            int pageCount = totalUserCount / pageSize;
            pageCount += (totalUserCount % pageSize) != 0 ? 1 : 0;
            return pageCount;
        }


        [HttpPost("AddScoredFoods")]
        public IActionResult AddScoredFoods(ScoredFoods scoredFoods)
        {
            try
            {
                _adminService.AddScoredFoods(scoredFoods);
                return Ok("Scored foods added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateScoredFoods")]
        public IActionResult UpdateScoredFoods(string id, int score)
        {
            try
            {
                _adminService.UpdateScoredFoods(id, score);
                return Ok("Score updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteScoredFoods")]
        public IActionResult DeleteScoredFoods(string id)
        {
            try
            {
                _adminService.DeleteScoredFoods(id);
                return Ok("Scored food deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet("ShowAdminScoredFoods")]
        public IActionResult ShowAdminScoredFoods(int pageNumber = 1, int pageSize = 30)
        {
            try
            {
                var scoredFoods = _adminService.ShowAdminScoredFoods(pageNumber, pageSize);
                return Ok(scoredFoods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SearchScoredFoodsByUserEmail")]
        public IActionResult SearchScoredFoodsByUserEmail(string userEmail, int pageNumber = 1, int pageSize = 30)
        {
            try
            {
                var scoredFoods = _adminService.SearchScoredFoodsByUserEmail(userEmail, pageNumber, pageSize);
                return Ok(scoredFoods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchScoredFoodsByFoodName")]
        public IActionResult SearchScoredFoodsByFoodName(string foodName, int pageNumber = 1, int pageSize = 30)
        {
            try
            {
                var scoredFoods = _adminService.SearchScoredFoodsByFoodName(foodName, pageNumber, pageSize);
                return Ok(scoredFoods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SearchScoredFoods")]
        public IActionResult SearchScoredFoods(string searchTerm, int pageNumber = 1, int pageSize = 30)
        {
            try
            {
                var scoredFoods = _adminService.SearchScoredFoods(searchTerm, pageNumber, pageSize);
                return Ok(scoredFoods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllUserEmails")]
        public IActionResult GetAllUserEmails()
        {
            try
            {
                var userNames = _adminService.GetAllUserEmails();
                return Ok(userNames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllFoodsNames")]
        public IActionResult GetAllFoodNames()
        {
            try
            {
                var foodNames = _adminService.GetAllFoodNames();
                return Ok(foodNames);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }
        [HttpPost("AddAdminScoredFoods")]
        public IActionResult AddScoredFoods([FromBody] AddScoredFoodsRequest request)
        {
            try
            {
                _adminService.AddAdminScoredFoods(request.UserEmail, request.FoodName, request.Score);
                return Ok("Scored food added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetScoredFoodWithId")]
        public IActionResult GetScoredFoodWithId(string scoredFoodId)
        {
            try
            {
                var scoredFood = _adminService.GetScoredFoodWithId(scoredFoodId);
                if (scoredFood == null)
                {
                    return NotFound(); // Eğer puanlanmış yiyecek bulunamazsa 404 Not Found döndür
                }

                return Ok(scoredFood);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CheckScoredFood")]
        public IActionResult CheckScoredFood(string userEmail, string foodName)
        {
            try
            {
                var result = _adminService.CheckScoredFood(userEmail, foodName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
