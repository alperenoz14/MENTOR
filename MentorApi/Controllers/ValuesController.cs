using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MENTOR.Models;
using MENTOR.Controllers;
using Newtonsoft.Json;

namespace MentorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        // GET api/values
        [HttpPost]
        public async Task Signup([FromBody]Mentor mentor)
        {
            using (var client = new HttpClient())
            {
                var data = JsonConvert.SerializeObject(mentor);
                HttpContent content = new StringContent(data, System.Text.Encoding.UTF8, "appication/json");
                await client.PostAsync("http://localhost:3000",content);
            }   

        } //to be continued...

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
