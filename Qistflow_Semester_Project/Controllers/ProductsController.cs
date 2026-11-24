using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qistflow_Semester_Project.Models;
using Qistflow_Semester_Project.Models.Interfaces;
using Qistflow_Semester_Project.Models.ViewModel;
namespace QistFlow.Controllers;

[Authorize(Roles = "Admin,Customer")]
public class ProductsController : Controller
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public ProductsController(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Search(string search)
    {
        List<Product> products = await _products.SearchProduct(search);
        return PartialView("_ProductGrid", products);
    }

    public async Task<IActionResult> Details(int id)
    {
        Product product = await _products.GetProductById(id);
        //if (product == null) return NotFound();

        ProductDetails page = new ProductDetails { Product = product };
        if (User.IsInRole("Admin")) page.Categories = await _categories.GetAllCategories();
        return View(page);
    }
}
