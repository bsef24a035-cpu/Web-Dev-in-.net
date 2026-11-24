namespace Qistflow_Semester_Project.Models.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProducts();
        public Task<Product> GetProductById(int productId);
        public Task<List<Product>> GetProductsByCategoryId(int categoryId);
        public Task AddProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(int productId);
        public Task<List<Product>> SearchProduct(string searchWord);
    }
}
