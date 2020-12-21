using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MENTOR.Controllers
{
    public class StudentController : Controller
    {

        public IActionResult Homepage()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Mentors()
        {
            return View();
        }
    }
}