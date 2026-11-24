namespace Qistflow_Semester_Project.Models.ViewModel
{
    public class ProductDetails
    {
        public Product Product { get; set; } = new();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
