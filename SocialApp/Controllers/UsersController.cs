﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Interface;
using SocialApp.Domain.Dtos;

namespace SocialApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISocialAppBusiness _dataBusiness;

        public UsersController(ISocialAppBusiness dataBusiness)
        {
            _dataBusiness = dataBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _dataBusiness.GetUsers());
        }

        [HttpGet("{id}")]
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