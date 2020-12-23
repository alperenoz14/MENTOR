using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MENTOR.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace MENTOR.Controllers
{
    public class StudentController : Controller
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
                //test edilecek...
                // loginden studentId session cekilecek..
                var response = await client.GetAsync("http://localhost:3000/student/getProfileInfo/" /*studentId*/);
                string responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Student>(responseContent);
                // result.studentId = sessiondan gelen Id...
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response != null)
                {
                    return View(result);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(Student student)
        {
            using (var client = new HttpClient())
            {
                //loginden gelen studentId sessionu çekilecek...
                //test edilecek(updated?)...
                var content = JsonConvert.SerializeObject(student);
                HttpContent formContent  = new StringContent(content,
                    System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/student/updateProfile",//+ studentId,
                                                        formContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Student>(responseContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK && result != null)
                {
                    return View(result);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        } 
        public IActionResult Mentors()
        {
            return View();
        }
    }
}