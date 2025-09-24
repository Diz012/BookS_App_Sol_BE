using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using MongoDB.Driver;

namespace BookS_Be.Repositories;

public class AuthorRepository(AppDbContext context) : IAuthorRepository
{
    public async Task<List<Author>> GetAllAsync()
    {
        return await context.Authors.Find(_ => true).ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(string id)
    {
        return await context.Authors.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Author?> GetByNameAsync(string name)
    {
        return await context.Authors.Find(a => a.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Author author)
    {
        await context.Authors.InsertOneAsync(author);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Authors.DeleteOneAsync(a => a.Id == id);
    }
}