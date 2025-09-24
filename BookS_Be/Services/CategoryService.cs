using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await categoryRepository.GetAllAsync();

    }

    public async Task AddCategoryAsync(Category category)
    {
        await categoryRepository.CreateAsync(category);
    }

    public async Task DeleteCategoryAsync(string categoryId)
    {
        await categoryRepository.DeleteAsync(categoryId);
    }
}