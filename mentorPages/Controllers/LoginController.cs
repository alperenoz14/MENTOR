using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MENTOR.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MENTOR.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            using (var client = new HttpClient())
            {
                IList<KeyValuePair<string, string>> userCollection = new List<KeyValuePair<string, string>> {
    { new KeyValuePair<string, string>("email",user.Email) },
    { new KeyValuePair<string, string>("password", user.Password) }
                };
                var result = await client.PostAsync("http://localhost:3000/mentor/login", new FormUrlEncodedContent(userCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                //var id = JObject.Parse(resultContent);
                var response = JsonConvert.DeserializeObject<Mentor>(resultContent);
                HttpContext.Session.SetInt32("mentorId", response.mentorId);
                //id sessionda tutulacak ve diger servis requestlerine parametre olarak geçecek...
                //student/mentor login işlemleri halledilecek...
                //roller :(...
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return StatusCode(200, response);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, response);
                }
            }
            return StatusCode(404);
        }
        
    }
}