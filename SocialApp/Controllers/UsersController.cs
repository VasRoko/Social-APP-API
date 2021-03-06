﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Helpers;
using SocialApp.Business.Interface;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using SocialApp.Helpers;

namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController : ControllerBase
    {
        private readonly IAppBusiness _dataBusiness;

        public UsersController(IAppBusiness dataBusiness)
        {
            _dataBusiness = dataBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await _dataBusiness.GetUsers(userParams, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var usersToReturn = _dataBusiness.MapUsers(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _dataBusiness.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), id));
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

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            string response = await _dataBusiness.Like(id, recipientId);

            if (response == null)
            {
                return NotFound();
            }

            if (response == "Ok")
            {
                return Ok();
            }

            return BadRequest(response);
        }
    }
}