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
using MENTOR.ApiModel;

namespace MENTOR.Controllers
{
    public class MentorController : Controller
    {
        Homepage homepageDatas = new Homepage();
        [HttpGet]
        public async Task<IActionResult> Homepage()
        {
            using (var client = new HttpClient())
            {
                var id = HttpContext.Session.GetInt32("mentorId");

                var responseQlist = await client.GetAsync("http://localhost:3000/mentor/getQuestions/" + id);
                var responseStudent = await client.GetAsync("http://localhost:3000/mentor/getStudents/" + id);

                if (responseQlist.StatusCode == System.Net.HttpStatusCode.OK && 
                        responseStudent.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await responseQlist.Content.ReadAsStringAsync();
                    var resultQuestions = JsonConvert.DeserializeObject<List<Question>>(responseContent);
                    homepageDatas.Questions = resultQuestions;
                    //student liste olarak mı gelecek yoksa 1-1 eşleşme mi olacak ?? ben şu an birebir gibi yapıyorum...
                    //eğer bire çok derlerse homepage data classına student Ienumerable olarak eklenecek...

                    string responseStudents = await responseStudent.Content.ReadAsStringAsync();
                    var resultStudent = JsonConvert.DeserializeObject<List<Student>>(responseStudents);
                    foreach (var item in resultStudent)
                    {
                        if (item.branchId == 1) item.branch = "Web Programlama";
                        else if (item.branchId == 2) item.branch = "Mobil Programlama";
                        else if (item.branchId == 3) item.branch = "Veri Bilimi";
                        else if (item.branchId == 4) item.branch = "Yapay Zeka/Makine Öğrenmesi";
                        else if (item.branchId == 5) item.branch = "Genel Tavsiye";
                    }
                    homepageDatas.Students = resultStudent;
                    return View(homepageDatas);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Homepage(Answer answer)
        {
            using (var client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(answer);
                HttpContent dataContent = new StringContent(content, 
                    System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/mentor/answerQuestion/" /* + questionId*/,dataContent);
                //hangi soruya cevap verdiğini anlaması için questionid göndermesi gerekmezmi?...
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
                //branches bastırılacak...
                var id = HttpContext.Session.GetInt32("mentorId");
                var responseMentor = await client.GetAsync("http://localhost:3000/mentor/getProfileInfo/" + id);
                var responseBranchs = await client.GetAsync("http://localhost:3000/getAllBranchs");

                if (responseMentor.StatusCode == System.Net.HttpStatusCode.OK &&
                    responseBranchs.StatusCode == System.Net.HttpStatusCode.OK && responseMentor != null)
                {
                    string responseMentorContent = await responseMentor.Content.ReadAsStringAsync();
                    var resultMentor = JsonConvert.DeserializeObject<Mentor>(responseMentorContent);
                    resultMentor.mentorId = Convert.ToInt32(id);
                    if (resultMentor.branchId == 1) resultMentor.branch = "Web Programlama";
                    else if (resultMentor.branchId == 2) resultMentor.branch = "Mobil Programlama";
                    else if (resultMentor.branchId == 3) resultMentor.branch = "Veri Bilimi";
                    else if (resultMentor.branchId == 4) resultMentor.branch = "Yapay Zeka/Makine Öğrenmesi";
                    else if (resultMentor.branchId == 5) resultMentor.branch = "Genel Tavsiye";

                    string responseBranchContent = await responseBranchs.Content.ReadAsStringAsync();
                    var resultBranches = JsonConvert.DeserializeObject<List<Branch>>(responseBranchContent);
                    resultMentor.AllBranches = resultBranches;
                    return View(resultMentor);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(Mentor mentor)
        {//yeni datalarla viewin doldurulup doldurulmadıgı test edilecek(updated)...
            using (var client = new HttpClient())
            {
                mentor.mentorId = Convert.ToInt32(HttpContext.Session.GetInt32("mentorId"));
                mentor.branchId = Convert.ToInt32(mentor.branch);
                //password problem...
                var content = JsonConvert.SerializeObject(mentor);
                HttpContent formContent = new StringContent(content,
                    System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/mentor/updateProfile/" + mentor.mentorId,
                                                                                                                 formContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseContent != null)
                {
                    return RedirectToAction("Profile", "Mentor");
                }
                else
                {
                    return StatusCode(Convert.ToInt16(response.StatusCode));
                }
            }
        }
    }
}