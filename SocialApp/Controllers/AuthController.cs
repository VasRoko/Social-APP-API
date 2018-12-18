using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Business;
using SocialApp.Models;


namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _config;

        public AuthController(IUserManager userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {

            if (await _userManager.Register(userRegisterDto.Username, userRegisterDto.Password) != null)
            {
                return StatusCode(201);
            }

            return BadRequest("Username already exist!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var applicationUser = await _userManager.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);

            if (applicationUser == null)
            {
                return Unauthorized();
            }

            return Ok(new {
                token = _userManager.TokenIssuer(applicationUser)
            });
        }

    }
}
