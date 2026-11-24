using Dapper;
using Qistflow_Semester_Project.Data;
using Qistflow_Semester_Project.Models.Interfaces;
using Qistflow_Semester_Project.Models.Repositories;

namespace Qistflow_Semester_Project.Models.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly IDbConnectionFactory _factory;
        public ProductRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            const string sql = """SELECT * FROM dbo.Products WHERE IsActive = 1 ORDER BY ProductId DESC""";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            List<Product> products = (await connection.QueryAsync<Product>(sql)).ToList();
            return products;
        }
        public async Task<Product> GetProductById(int productId)
        {
            string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            Product product = await connection.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = productId });
            return product;
        }
        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            string query = "SELECT * FROM Products WHERE CategoryId = @CategoryId";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            List<Product> products = (await connection.QueryAsync<Product>(query, new { CategoryId = categoryId })).ToList();
            return products;
        }
        public async Task AddProduct(Product product)
        {
            try
            {
                string query = "INSERT INTO Products (CategoryId, Name, Brand, Description, CashPrice, InstallmentPrice, StockQuantity, ImageUrl, isActive, InstallmentMonths, DownPayment) VALUES (@CategoryId, @Name, @Brand, @Description, @CashPrice, @InstallmentPrice, @StockQuantity, @ImageUrl, @isActive, @InstallmentMonths, @DownPayment)";
                await using var connection = _factory.CreateConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { CategoryId = product.categoryId, Name = product.Name, Brand = product.Brand, Description = product.Description, CashPrice = product.CashPrice, InstallmentPrice = product.InstallmentPrice, StockQuantity = product.StockQuantity, ImageUrl = product.ImageUrl, isActive = 1, InstallmentMonths = product.InstallmentMonths, DownPayment = product.DownPayment});
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, rethrow it, etc.)
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }
        public async Task UpdateProduct(Product product)
        {
            try {
                string query = "UPDATE Products SET CategoryId = @CategoryId, Name = @Name, Brand = @Brand, Description = @Description, CashPrice = @CashPrice, InstallmentPrice = @InstallmentPrice, StockQuantity = @StockQuantity, ImageUrl = @ImageUrl, DownPayment=@DownPayment WHERE ProductId = @ProductId";
                await using var connection = _factory.CreateConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { CategoryId = product.categoryId, Name = product.Name, Brand = product.Brand, Description = product.Description, CashPrice = product.CashPrice, InstallmentPrice = product.InstallmentPrice, StockQuantity = product.StockQuantity, ImageUrl = product.ImageUrl, ProductId = product.productId, DownPayment = product.DownPayment});

            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, rethrow it, etc.)
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }
        public async Task DeleteProduct(int productId)
        {
            string query = "DELETE FROM Products WHERE ProductId = @ProductId";
            await using var connection = _factory.CreateConnection();   
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, new { ProductId = productId });
        }

        public async Task<List<Product>> SearchProduct(string searchWord)
        {
            string query = "SELECT * FROM Products WHERE Name LIKE '%' + @SearchWord + '%' OR Description LIKE '%' + @SearchWord + '%'";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            List<Product> products = (await connection.QueryAsync<Product>(query, new {SearchWord = searchWord})).ToList();
            return products;
        }
    }
}
