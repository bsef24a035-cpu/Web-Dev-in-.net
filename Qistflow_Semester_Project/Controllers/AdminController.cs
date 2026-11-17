using Microsoft.AspNetCore.Mvc;

namespace Qistflow_Semester_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
