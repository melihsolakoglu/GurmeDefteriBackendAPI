using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Services;
using GurmeDefteriBackEndAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace GurmeDefteriBackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtSettings _jwtSettings;
        private readonly DailyActivityCounterService _dailyActivityCounterService;

        public AuthController(IOptions<JwtSettings> jwtSettings,DailyActivityCounterService dailyActivityCounterService)
        {
            _authService = new AuthService();
            _jwtSettings = jwtSettings.Value;
            _dailyActivityCounterService = dailyActivityCounterService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginUser logUser)
        {
            if (_authService.ValidateUser(logUser))
            {
                User user = _authService.FindUser(logUser.Email, logUser.Password);
                var token = CreateToken(user);
                Log.Information("Kullanıcı giriş yaptı: {UserName}", logUser.Email);
                _dailyActivityCounterService.IncrementLoginCount();
                return Ok(token);
            }
            Log.Information("Kullanıcı girişi başarısız: {UserName}", logUser.Email);
            return Unauthorized(new { Response = false });
        }

        [HttpPost("AdminLogin")]
        public ActionResult AdminLogin([FromBody] LoginUser logUser)
        {
            if (_authService.IsAdmin(logUser) && _authService.ValidateUser(logUser))
            {
                User user = _authService.FindUser(logUser.Email, logUser.Password);
                var token = CreateToken(user);
                Log.Information("Admin giriş yaptı: {UserName}", logUser.Email);
                _dailyActivityCounterService.IncrementLoginCount();
                return Ok(token);
            }
            Log.Information("Admin girişi başarısız: {UserName}", logUser.Email);
            return Unauthorized(new { Response = false });
        }

        private string CreateToken(User user)
        {
            if (_jwtSettings.Key == null) throw new Exception("Jwt Key value cannot be null");


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimArray = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claimArray,
                expires: DateTime.UtcNow.AddYears(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}