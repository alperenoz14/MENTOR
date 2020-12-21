using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace MENTOR.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Homepage()
        {
            return View();
        }
    }
}