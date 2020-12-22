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
        enum branches
        {
            WebProgramlama = 1,
            MobilProgramlama = 2,
            VeriBilimi = 3,
            MakineÖğrenmesi = 4,
            GenelTavsiye = 5
        }
        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            ViewBag.branches = new List<string>() { "Web Programlama", "Mobil Programlama", "Veri Bilimi", "Yapay Zeka/Makine Öğrenmesi", "Genel Tavsiye" };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(Mentor mentor)
        {   //mentorbranchId'lerinin çekimi...
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
    { new KeyValuePair<string, string>("password", mentor.password) },
    { new KeyValuePair<string, string>("email", mentor.email) },
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
            ViewBag.branches = new List<string>() { "Web Programlama", "Mobil Programlama", "Veri Bilimi", "Yapay Zeka/Makine Öğrenmesi", "Genel Tavsiye" };
            var branch = student.branch;
            if (branch == "Web Programlama") student.branchId = 1;
            else if (branch == "Mobil Programlama") student.branchId = 2;
            else if (branch == "Veri Bilimi") student.branchId = 3;
            else if (branch == "Yapay Zeka/Makine Öğrenmesi") student.branchId = 4;
            else if (branch == "Genel Tavsiye") student.branchId = 5;
            using (var client = new HttpClient())
                {
                    /*IList<KeyValuePair<string, object>> studentCollection = new List<KeyValuePair<string, object>> {
    { new KeyValuePair<string, object>("ad",student.Name) },
    { new KeyValuePair<string, object>("soyad",student.LastName) },
    { new KeyValuePair<string, object>("password", student.Password) },
    { new KeyValuePair<string, object>("email", student.Email) },
    { new KeyValuePair<string, object>("branchId",student.branchId ) },

                };*/
                var content = JsonConvert.SerializeObject(student);
                HttpContent formContent = new StringContent(content,
                System.Text.Encoding.UTF8, "application/json");
                var result = await client.PostAsync("http://localhost:3000/student/register",formContent);
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