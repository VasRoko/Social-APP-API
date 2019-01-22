using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialApp.Business.Interface;
using SocialApp.Domain.Dtos;
using SocialApp.Models;


namespace SocialApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthManager userManager,
            IConfiguration config,
            IMapper mapper )
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
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
            var user = await _userManager.Login(userLoginDto.Username, userLoginDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

    }
}
