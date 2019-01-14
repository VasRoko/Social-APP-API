using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Helpers;
using SocialApp.Business.Interface;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using SocialApp.Helpers;

namespace SocialApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ISocialAppBusiness _businessRepo;
        private readonly IMessageManager _messageManager;

        public MessagesController(ISocialAppBusiness businessRepo, IMessageManager messageManager)
        {
            _businessRepo = businessRepo;
            _messageManager = messageManager;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userid, int id)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _messageManager.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int userid, [FromQuery]MessageParams messageParams)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messagesPagination = await _messageManager.GetMessageForUser(userid, messageParams);
            var returnMessages = _messageManager.MapMessagesForUser(messagesPagination);

            Response.AddPagination(messagesPagination.CurrentPage, messagesPagination.PageSize, messagesPagination.TotalCount, messagesPagination.TotalPages);

            return Ok(returnMessages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThred(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _messageManager.GetMessageThred(userId, recipientId);
            return Ok(message);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userid, MessageForCreactionDto messageForCreaction)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _messageManager.CreateMessage(userid, messageForCreaction);
            var messageForReturn = _messageManager.MessageForCreationRetun(message);
            
            if (message == null)
            {
                return BadRequest("Could not find user");
            }

            return CreatedAtRoute("GetMessage", new { id = message.Id }, messageForReturn);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userid)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (await _messageManager.DeleteMessage(id, userid) == null)
            {
                return NoContent();
            }

            return BadRequest("Could not delete your message");
        }

    }
}