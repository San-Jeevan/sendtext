using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiAndWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

   
        public IActionResult Session(int id)
        {
            ViewData["Message"] = "Your contact page." + id;

            return View();
        }

   
    }
}
