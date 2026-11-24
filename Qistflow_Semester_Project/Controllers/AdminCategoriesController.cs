using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Qistflow_Semester_Project.Models.Interfaces;

namespace Qistflow_Semester_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Categories")]
    public class AdminCategoriesController : Controller
    {
        private readonly ICategoryRepository _categories;
        public AdminCategoriesController(ICategoryRepository categories) => _categories = categories;

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, string Description)
        {
            if (string.IsNullOrWhiteSpace(Name))
                ModelState.AddModelError(nameof(Name), "Category name is required.");
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = string.Join(" ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                });
            try
            {
                await _categories.AddCategory(new Models.Category { Name = Name, Description = Description });
                return RedirectToAction("Stock", "Admin");
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                return Conflict(new { message = "This category already exists." });
            }
        }
    }
}
