using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Interface;

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
    }
}