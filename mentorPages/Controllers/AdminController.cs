using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MENTOR.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MENTOR.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            using (var client = new HttpClient())
            {
                IList<KeyValuePair<string, string>> userCollection = new List<KeyValuePair<string, string>> {
    { new KeyValuePair<string, string>("email",admin.email) },
    { new KeyValuePair<string, string>("password", admin.password) }
                };

                var result = await client.PostAsync("http://localhost:3000/admin/login", new FormUrlEncodedContent(userCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                var adminData = JsonConvert.DeserializeObject<Admin>(resultContent);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    int adminId = adminData.adminId;
                    HttpContext.Session.SetInt32("adminId", adminId);
                    ClaimsIdentity identity = null;
                    identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role,"Admin")
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Homepage", "Admin");
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        Homepage data = new Homepage();
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Homepage()
        {
            var id = HttpContext.Session.GetInt32("adminId");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost:3000/getAllBranchs");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseBranch = await response.Content.ReadAsStringAsync();
                    var branches = JsonConvert.DeserializeObject<List<Branch>>(responseBranch);
                    data.Branches = branches;
                    return View(data);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Homepage(Branch branch)
        {
            
            using (var client = new HttpClient())
            {
                var data = JsonConvert.SerializeObject(branch);
                HttpContent content = new StringContent(data, 
                    System.Text.Encoding.UTF8, "application/json");
                //APİ returns 404 ??
                var response = await client.PostAsync("http://localhost:3000/admin/addBranch", content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Homepage", "Admin");
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }
    }
}