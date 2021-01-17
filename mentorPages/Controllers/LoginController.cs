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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MENTOR.Controllers
{
    [AllowAnonymous]
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

                var result = await client.PostAsync("http://localhost:3000/login", new FormUrlEncodedContent(userCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic json = JsonConvert.DeserializeObject(resultContent);
                    if (json.role == "mentor")
                    {
                        int mentorId = Convert.ToInt32(json.mentorId);
                        HttpContext.Session.SetInt32("mentorId", mentorId);
                        ClaimsIdentity identity = null;
                        identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Role,"Mentor")
                        },CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return RedirectToAction("Homepage", "Mentor");
                    }
                    else if (json.role == "student")
                    {
                        int studentId = Convert.ToInt32(json.studentId);
                        HttpContext.Session.SetInt32("studentId", studentId);
                        ClaimsIdentity identity = null;
                        identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Role,"Student")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return RedirectToAction("Homepage", "Student");
                    }
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, 0);
                }
            }
            return StatusCode(404);
        }

    }
}