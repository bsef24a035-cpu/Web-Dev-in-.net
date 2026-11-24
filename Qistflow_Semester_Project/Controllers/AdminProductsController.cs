using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Qistflow_Semester_Project.Models.Interfaces;
using Qistflow_Semester_Project.Models;
using Qistflow_Semester_Project.Models.ViewModel;

namespace QistFlow.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public AdminProductsController(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    // The Add and Update forms share the same input names.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int productId, string Name, int categoryId,
        string? Brand, string? Description, string? sku, decimal CashPrice,
        int StockQuantity, decimal InstallmentPrice, decimal DownPayment,
        int DurationMonths)
    {
        Product product = new Product
        {
            productId = productId,
            Name = Name?.Trim() ?? "",
            Description = Description,
            Brand = Brand,
            categoryId = categoryId,
            CashPrice = CashPrice,
            StockQuantity = StockQuantity,
            InstallmentPrice = InstallmentPrice,
            DownPayment = DownPayment,
            InstallmentMonths = DurationMonths,
            ImageUrl = "#"
        };

        //if (product.productName.Length == 0 || product.productName.Length > 160 || brand?.Length > 80)
        //    ModelState.AddModelError("", "Check the product name, brand and SKU.");
        //if (cashPrice <= 0 || stockQuantity < 0)
        //    ModelState.AddModelError("", "Choose a category, enter a positive cash price and check stock.");
        //if (durationMonths < 1 || durationMonths > 60 || totalPayable <= 0 || downPayment < 0 || downPayment > (totalPayable))
        //    ModelState.AddModelError("", "Check installment price, down payment and months (1–60).");

        try
        {
            if (productId == 0)
            {
                await _products.AddProduct(product);
                Console.WriteLine("Product added.");
            }
            else
                await _products.UpdateProduct(product);
            return RedirectToAction("Stock", "Admin");

            //return RedirectToAction("Details", "Products", new { id = productId });
        }
        catch (SqlException error) when (error.Number == 2601 || error.Number == 2627)
        {
            ModelState.AddModelError("", "A record with these details already exists. Check the SKU.");
        }

        return RedirectToAction("Stock", "Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int productId)
    {
        await _products.DeleteProduct(productId);
        return RedirectToAction("Stock", "Admin");
    }
}
