using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MENTOR.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MENTOR.Controllers
{
    [Authorize(Roles ="Student,Admin")]
    public class StudentController : Controller
    {
        
        Homepage homepageDatas = new Homepage();

        [HttpGet]
        public async Task<IActionResult> Homepage()
        {

            using (var client = new HttpClient())
            {
                var id = HttpContext.Session.GetInt32("studentId");
                var responseQlist = await client.GetAsync("http://localhost:3000/student/getQuestionList/" + id);
                var responseMentorInfo = await client.GetAsync("http://localhost:3000/student/getMentorInfo/" + id);
                //mentor info eksik geliyor...
                if (responseQlist.StatusCode == System.Net.HttpStatusCode.OK &&
                        responseMentorInfo.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await responseQlist.Content.ReadAsStringAsync();
                    var resultQuestions = JsonConvert.DeserializeObject<List<Question>>(responseContent);
                    homepageDatas.Questions = resultQuestions;

                    string responseContentMentor = await responseMentorInfo.Content.ReadAsStringAsync();
                    //branch gelme sorunu çözülecek...
                    var resultMentorInfo = JsonConvert.DeserializeObject<Mentor>(responseContentMentor);
                    if (resultMentorInfo.branchId == 1) resultMentorInfo.branch = "Web Programlama";
                    else if (resultMentorInfo.branchId == 2) resultMentorInfo.branch = "Mobil Programlama";
                    else if (resultMentorInfo.branchId == 3) resultMentorInfo.branch = "Veri Bilimi";
                    else if (resultMentorInfo.branchId == 4) resultMentorInfo.branch = "Yapay Zeka/Makine Öğrenmesi";
                    else if (resultMentorInfo.branchId == 5) resultMentorInfo.branch = "Genel Tavsiye";
                    homepageDatas.MentorInfo = resultMentorInfo;
                    return View(homepageDatas);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Homepage(Question question)
        {
            using (var client = new HttpClient())
            {
                var id = HttpContext.Session.GetInt32("studentId");
                //student ıd'sini sessiondan al sonra studentInfosuna get istek at gelen ıd değerleri ile questionu doldur,yolla...
                //id işlemleri ve post işleminden gelen data kontrol edilecek...
                //hangi soruya cevap verdiğini nasıl anlayacak?...
                //question post olduğunda question id versin bana aynı logindeki gibi...
                //question id belirsizlik...
                var info = await client.GetAsync("http://localhost:3000/student/getProfileInfo/" + id);
                string res = await info.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(res);
                question.studentId = Convert.ToInt32(id);
                question.date = DateTime.Now;
                question.branchId = student.branchId;
                question.mentorId = student.mentorId;

                var content = JsonConvert.SerializeObject(question);
                HttpContent dataContent = new StringContent(content, 
                          System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/student/addQuestion/" + question.studentId,
                                                                                                                   dataContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseContent != null)
                {
                    return RedirectToAction("Homepage", "Student");
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            using (var client = new HttpClient())
            {
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
                student.studentId = Convert.ToInt32(HttpContext.Session.GetInt32("studentId"));
                student.branchId = Convert.ToInt32(student.branch);
                var content = JsonConvert.SerializeObject(student);
                HttpContent formContent  = new StringContent(content,
                    System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/student/updateProfile/" + student.studentId,
                                                                                                                    formContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseContent != null)
                {
                    return RedirectToAction("Profile","Student");
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpPost]
        [ActionName("Signout")]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}