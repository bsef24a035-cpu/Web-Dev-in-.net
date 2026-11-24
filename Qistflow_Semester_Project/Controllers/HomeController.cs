using Microsoft.AspNetCore.Mvc;
using Qistflow_Semester_Project.Models;
using System.Diagnostics;

namespace Qistflow_Semester_Project.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
