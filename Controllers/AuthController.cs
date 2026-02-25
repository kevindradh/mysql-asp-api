using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CobaMySql.Data;
using CobaMySql.DTOs;
using CobaMySql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CobaMySql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Username == dto.Username);

            if (user == null)
                return Unauthorized("Username atau password salah.");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid)
                return Unauthorized("Username atau password salah.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.Id,
                    user.Username
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var key = _config["JWT:Key"];

            var setClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            
            var setSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var setCredentials = new SigningCredentials(setSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: setClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: setCredentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
