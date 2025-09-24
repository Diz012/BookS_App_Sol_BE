using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using MongoDB.Driver;

namespace BookS_Be.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<List<Category>> GetAllAsync()
    {
        return await context.Categories.Find(_ => true).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(string id)
    {
        return await context.Categories.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await context.Categories.Find(c => c.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Category category)
    {
        await context.Categories.InsertOneAsync(category);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Categories.DeleteOneAsync(c => c.Id == id);
    }
}