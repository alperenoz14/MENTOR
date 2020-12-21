using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MENTOR.Models;
using System.Net.Http;
using Newtonsoft.Json;


namespace MENTOR.Controllers
{
    public class SignupController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(Mentor mentor)
        {
            using (var client = new HttpClient())
            {
                /*client.BaseAddress = new Uri("http://localhost:63744/api/MyApi");
                var data = JsonConvert.SerializeObject(mentor);
                HttpContent content = new StringContent(data,
                System.Text.Encoding.UTF8, "application/json");
                var result = await client.PostAsync("http://localhost:63744/api/MyApi/", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return StatusCode(200, data);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, result);
                }*/
                IList<KeyValuePair<string, string>> mentorCollection = new List<KeyValuePair<string, string>> {
    { new KeyValuePair<string, string>("ad",mentor.Name) },
    { new KeyValuePair<string, string>("soyad",mentor.LastName) },
    { new KeyValuePair<string, string>("description",mentor.Description) },
    { new KeyValuePair<string, string>("password", mentor.Password) },
    { new KeyValuePair<string, string>("email", mentor.Email) },
                };
                //client.BaseAddress = new Uri("http://localhost:3000/mentor/register");
                var result = await client.PostAsync("http://localhost:3000/mentor/register", new FormUrlEncodedContent(mentorCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return StatusCode(200, resultContent);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, resultContent);
                }
            }
            return StatusCode(404);
        }
        [HttpPost]
        [ActionName("signUpstudent")]
        public async Task<IActionResult> Signup(Student student)
        {
            using (var client = new HttpClient())
            {
                IList<KeyValuePair<string, string>> studentCollection = new List<KeyValuePair<string, string>> {
    { new KeyValuePair<string, string>("ad",student.Name) },
    { new KeyValuePair<string, string>("soyad",student.LastName) },
    { new KeyValuePair<string, string>("password", student.Password) },
    { new KeyValuePair<string, string>("email", student.Email) },
                };
                //client.BaseAddress = new Uri("http://localhost:3000/student/register");
                var result = await client.PostAsync("http://localhost:3000/student/register", new FormUrlEncodedContent(studentCollection));
                string resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return StatusCode(200, resultContent);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return StatusCode(404, resultContent);
                }
            }
            return StatusCode(404);
        }
    }
}