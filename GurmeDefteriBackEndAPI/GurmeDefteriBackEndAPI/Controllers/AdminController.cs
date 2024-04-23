using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GurmeDefteriBackEndAPI.Models;
using GurmeDefteriBackEndAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<List<User>> GetAllFoods()
        {
            var foods = _adminService.GetFoods();
            return Ok(foods);
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
        public IActionResult AddFood([Required, StringLength(70, MinimumLength = 2, ErrorMessage = "İsim en az 2, en fazla 70 karakter olmalıdır.")] string name,
                                    [Required(ErrorMessage = "Ülke alanı girilmelidir.")] string country,
                                    [Required(ErrorMessage = "Resim alanı girilmelidir.")] IFormFile image)
        {
            try
            {
                _adminService.AddFood(name, country, image);
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
