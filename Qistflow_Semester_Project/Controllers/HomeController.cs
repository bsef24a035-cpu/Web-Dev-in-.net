using Microsoft.AspNetCore.Mvc;
using Qistflow_Semester_Project.Models;
using System.Diagnostics;

namespace Qistflow_Semester_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            return View();
        }
    }
}
