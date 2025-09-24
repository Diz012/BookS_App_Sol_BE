using BookS_Be.Models;

namespace BookS_Be.Services.Interfaces;

public interface ICategoryService
{ 
    Task<List<Category>> GetCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task DeleteCategoryAsync(string categoryId);
}