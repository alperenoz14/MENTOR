﻿using System;
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
        public IActionResult Homepage()
        {
            return View();
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
                
                //result.mentorId = Convert.ToInt32(id);
                if (responseMentor.StatusCode == System.Net.HttpStatusCode.OK &&
                    responseBranchs.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseMentorContent = await responseMentor.Content.ReadAsStringAsync();
                    var resultMentor = JsonConvert.DeserializeObject<Mentor>(responseMentorContent);

                    string responseBranchContent = await responseBranchs.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<Branch>>(responseBranchContent);
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
            var mentorid = HttpContext.Session.GetInt32("mentorId"); 
            using (var client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(mentor);
                HttpContent formContent = new StringContent(content,
                    System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:3000/mentor/updateProfile" + 
                                                        mentorid, formContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Mentor>(responseContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK && result != null)
                {
                    return View(result);
                }
                else
                {
                    return StatusCode(404,result.mentorId);
                }
            }
        }
    }
}