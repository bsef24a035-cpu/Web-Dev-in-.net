using Qistflow_Semester_Project.Models.Interfaces;
using Qistflow_Semester_Project.Data;
using Dapper;

namespace Qistflow_Semester_Project.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _factory;
        public CategoryRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }
        public async Task<List<Category>> GetAllCategories(){
            try
            {
                string query = "SELECT * FROM Categories";
                await using var connection = _factory.CreateConnection();
                await connection.OpenAsync();
                List<Category> categories = (await connection.QueryAsync<Category>(query)).ToList();
                return categories;
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, rethrow it, return an empty list, etc.)
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }
        public async Task<Category> GetCategoryById(int categoryId){
            string query = "SELECT * FROM Categories WHERE Id = @CategoryId";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            var category = await connection.QuerySingleAsync<Category>(query, new { CategoryId = categoryId });
            return category;
        }
        public async Task AddCategory(Category category){
            string query = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, new { Name = category.Name, Description = category.Description});
        }
        public async Task UpdateCategory(Category category){
            string query = "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, new { Id = category.categoryId, Name = category.Name, Description = category.Description });
        }
        public async Task DeleteCategory(int categoryId){
            string query = "DELETE FROM Categories WHERE Id = @CategoryId";
            await using var connection = _factory.CreateConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, new { CategoryId = categoryId });
        }
    }
}
