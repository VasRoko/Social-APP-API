using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Helpers;
using SocialApp.Business.Interface;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using SocialApp.Helpers;

namespace SocialApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController : ControllerBase
    {
        private readonly ISocialAppBusiness _dataBusiness;

        public UsersController(ISocialAppBusiness dataBusiness)
        {
            _dataBusiness = dataBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await _dataBusiness.GetUsers(userParams, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var usersToReturn = _dataBusiness.Users(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _dataBusiness.GetUser(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (await _dataBusiness.UpdateUser(id, userForUpdateDto) == null)
            {
                throw new Exception($"Updating user {id} failed on save");
            }

            return NoContent();

        }
    }
}