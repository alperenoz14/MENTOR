using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MENTOR.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace MENTOR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        [HttpPost]
        public async Task PostData([FromBody]Mentor mentor)
        {
            if (mentor != null)
            {
                using (var client = new HttpClient())
                {
                    var data = JsonConvert.SerializeObject(mentor);
                    HttpContent content = new StringContent(data,
                    System.Text.Encoding.UTF8, "application/json");
                    await client.PostAsync("http://localhost:3000", content);//??
                }
            }
        }


    }
}