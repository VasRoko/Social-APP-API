using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business;
using SocialApp.Business.Interface;
using SocialApp.Domain.Dtos;

namespace SocialApp.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IPhotoManager _photoManager;

        public PhotosController(IUserManager userManager, IPhotoManager photoManager)
        {
            _userManager = userManager;
            _photoManager = photoManager;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            return Ok(await _photoManager.GetPhoto(id)); 
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var photo = await _photoManager.AddPhotoUser(userId, photoForCreationDto);

            if (photo != null)
            {
                return CreatedAtRoute("GetPhoto", new { id = photo.PublicId }, photo);
            }

            return BadRequest("Could not add the photo");
        }

      
    }
}