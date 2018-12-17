using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Business.Interface;
using SocialApp.Domain;

namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISocialAppBusiness _business;

        public ValuesController(ISocialAppBusiness business)
        {
            _business = business;
        }
        // GET api/values
        [HttpGet]
        public ActionResult GetValues()
        {
            var values = _business.BusinessTest();

            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult GetValue(int id)
        {
            return Ok(_business.GetValue(id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
