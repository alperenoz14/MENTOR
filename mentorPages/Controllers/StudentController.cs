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
                var id = HttpContext.Session.GetInt32("studentId");
                var response = await client.GetAsync("http://localhost:3000/student/getProfileInfo/" + id);
                var responseBranches = await client.GetAsync("http://localhost:3000/getAllBranchs");

                if (response.StatusCode == System.Net.HttpStatusCode.OK && 
                    responseBranches.StatusCode == System.Net.HttpStatusCode.OK && response != null)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var resultStudent = JsonConvert.DeserializeObject<Student>(responseContent);
                    if (resultStudent.branchId == 1) resultStudent.branch = "Web Programlama";
                    else if (resultStudent.branchId == 2) resultStudent.branch = "Mobil Programlama";
                    else if (resultStudent.branchId == 3) resultStudent.branch = "Veri Bilimi";
                    else if (resultStudent.branchId == 4) resultStudent.branch = "Yapay Zeka/Makine Öğrenmesi";
                    else if (resultStudent.branchId == 5) resultStudent.branch = "Genel Tavsiye";
                    resultStudent.studentId = Convert.ToInt32(id);

                    string responseBranchContent = await responseBranches.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<Branch>>(responseBranchContent);
                    resultStudent.AllBranches = result;
                    return View(resultStudent);
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