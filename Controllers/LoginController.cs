using DBAPP3.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DBAPP3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //var expectedAccessKey = "thisisaverystrongsecretkey123456789";
            //var expectedAccessKey = _configuration.GetSection("");
            var secretKey = Environment.GetEnvironmentVariable("SecretKey");
            var jwtSettings = _configuration.GetSection("JwtSettings");
            //var secretKey = jwtSettings["key"];



            if (request.AccessKey != secretKey)
                return Unauthorized("Invalid credentials");

            var key = Encoding.UTF8.GetBytes(secretKey);
            if (key.Length < 32)
                return StatusCode(500, "Secret key must be at least 32 characters (256 bits) long.");

            var tokenHandler = new JwtSecurityTokenHandler();



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "access-user"),
                    new Claim(ClaimTypes.Role, "Access")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtSettings["issuer"],
                Audience = jwtSettings["audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
