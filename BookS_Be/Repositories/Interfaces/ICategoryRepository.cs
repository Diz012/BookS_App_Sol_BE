using BookS_Be.Models;

namespace BookS_Be.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task <List<Category>> GetAllAsync();
    Task <Category?> GetByIdAsync(string id);
    Task <Category?> GetByNameAsync(string name);
    Task CreateAsync(Category category);
    Task DeleteAsync(string id);
}