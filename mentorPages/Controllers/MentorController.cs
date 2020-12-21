using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using MENTOR.Models;

namespace MENTOR.Controllers
{
    public class MentorController : Controller
    {
        public IActionResult Homepage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            using (var client = new HttpClient())
            {
                var id = HttpContext.Session.GetInt32("mentorId");
                var result = await client.GetAsync("http://localhost:3000/mentor/getProfileInfo/" + id);
                string resultContent = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Mentor>(resultContent);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return StatusCode(200, response.mentorId);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, response.mentorId);
                }
            }
            return StatusCode(404);
        }

        
    }
}