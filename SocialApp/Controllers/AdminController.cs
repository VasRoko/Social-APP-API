using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Interface;
using SocialApp.Domain.Dtos;

namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;

        public AdminController(IAdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("usersWithRoles")]
        public IActionResult GetUsersWithRoles()
        {
            return Ok(_adminManager.GetUsersWithRoles().Result);
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roles)
        {
            var result = await _adminManager.EditRoles(userName, roles);
            if (result == null)
            {
                return BadRequest("Failed to Add/Remove roles");
            }

            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public async Task<IActionResult> GetPhotosForModeration()
        { 
            return Ok(await _adminManager.GetPhotoForModeration());
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approvePhoto/{id}")]
        public async Task<IActionResult> ApprovePhoto(int id)
        {
            if (await _adminManager.ApprovePhoto(id))
            {
                return Ok();
            }

            return BadRequest("Failed to Approve the photo");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("rejectPhoto/{id}")]
        public async Task<IActionResult> RejectPhoto(int id)
        {
            if (await _adminManager.RejectPhoto(id))
            {
                return Ok();
            }

            return BadRequest("Failed to Reject the photo");
        }
    }
}