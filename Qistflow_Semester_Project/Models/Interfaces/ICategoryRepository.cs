namespace Qistflow_Semester_Project.Models.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategories();
        public Task<Category> GetCategoryById(int categoryId);
        public Task AddCategory(Category category);
        public Task UpdateCategory(Category category);
        public Task DeleteCategory(int categoryId);
    }
}
