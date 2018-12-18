using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business;
using SocialApp.Models;


namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public AuthController(IUserManager userManager)
        {
            _userManager = userManager;
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

    }
}
