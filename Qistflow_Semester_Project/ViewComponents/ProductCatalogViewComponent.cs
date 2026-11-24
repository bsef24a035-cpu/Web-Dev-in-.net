using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Qistflow_Semester_Project.Models.Interfaces;
using Qistflow_Semester_Project.Models.Repositories;
using Qistflow_Semester_Project.Models.ViewModel;

namespace Qistflow_Semester_Project.ViewComponents;

public sealed class ProductCatalogViewComponent : ViewComponent
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;
    public ProductCatalogViewComponent(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categories.GetAllCategories();
        var products = await _products.GetAllProducts();
        var vm = new ProductCatalog{Products = products, Categories = categories };
        return View(vm);
    }
}
