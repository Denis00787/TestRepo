using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OauthWebAPI.Models;

namespace OauthWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
	{
        private IConfiguration _config;

        public AuthController(IConfiguration configuration)
		{
            _config = configuration;
		}

        [HttpPost(Name = "Authorize")]
        public ActionResult AuthUser(User user)
        {
           var token = GenerateJwt();
            return Ok(token);
        }

        private string GenerateJwt()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //If you've had the login module, you can also use the real user information here
            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, "user_name"),
        new Claim(JwtRegisteredClaimNames.Email, "user_email"),
        new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddHours(2).ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

