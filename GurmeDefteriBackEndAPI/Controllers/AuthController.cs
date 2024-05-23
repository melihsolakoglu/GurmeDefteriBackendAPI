﻿using Microsoft.AspNetCore.Http;
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

        public AuthController(IOptions<JwtSettings> jwtSettings)
        {
            _authService = new AuthService();
            _jwtSettings = jwtSettings.Value;
        }
        [HttpPost]
        public ActionResult Login([FromBody] LoginUser logUser)
        {
            if (_authService.ValidateUser(logUser))
            {
                User user = _authService.FindUser(logUser.Email, logUser.Password);
                var token = CreateToken(user);
                Log.Information("User logged in: {UserName}", logUser.Email);
                return Ok(token);
            }
            Log.Information("User can't logged in: {UserName}", logUser.Email);
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