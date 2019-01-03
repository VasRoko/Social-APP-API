using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Business;
using SocialApp.Domain.Dtos;
using SocialApp.Models;


namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IUserManager userManager, IConfiguration config, IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            var createdUser = await _userManager.Register(_mapper.Map<UserForRegisterDto>(userRegister), userRegister.Password);

            if (createdUser == null)
            {
                return BadRequest("Username already exist!");
            }

            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLoginDto)
        {
            var user = await _userManager.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

    }
}
