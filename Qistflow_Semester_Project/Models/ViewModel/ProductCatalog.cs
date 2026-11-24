namespace Qistflow_Semester_Project.Models.ViewModel
{
    public class ProductCatalog
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public ProductDetails Form { get; set; } = new();
    }
}
