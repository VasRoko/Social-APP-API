using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business;
using SocialApp.Business.Interface;
using SocialApp.Domain.Dtos;

namespace SocialApp.Controllers
{
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IAuthManager _userManager;
        private readonly IPhotoManager _photoManager;

        public PhotosController(IAuthManager userManager, IPhotoManager photoManager)
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
        public async Task<IActionResult> AddPhotoUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
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

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var result = await _photoManager.SetMainPhoto(userId, id);

            if (!result.isValid)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var result = await _photoManager.DeletePhoto(userId, id);

            if (!result.isValid)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }


    }
}