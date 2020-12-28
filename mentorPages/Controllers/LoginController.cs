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
    { new KeyValuePair<string, string>("email",user.email) },
    { new KeyValuePair<string, string>("password", user.password) }
                };
                                                                 //loginde ayrım yapılıp studente atılacak...          
                var result = await client.PostAsync("http://localhost:3000/login", new FormUrlEncodedContent(userCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultContent);
                    if (json.role == "mentor")
                    {
                        int mentorId = Convert.ToInt32(json.mentorId);
                        HttpContext.Session.SetInt32("mentorId", mentorId);
                        return RedirectToAction("Profile", "Mentor");
                    }
                    else if (json.role == "student")
                    {
                        int studentId = Convert.ToInt32(json.studentId);
                        HttpContext.Session.SetInt32("studentId", studentId);
                        return RedirectToAction("Profile", "Student");
                    }
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404,0);
                }
            }
            return StatusCode(404);
        }
        
    }
}