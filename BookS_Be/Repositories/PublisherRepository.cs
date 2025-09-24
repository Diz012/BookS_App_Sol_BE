using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using MongoDB.Driver;

namespace BookS_Be.Repositories;

public class PublisherRepository(AppDbContext context) : IPublisherRepository
{
    public async Task<List<Publisher>> GetAllAsync()
    {
        return await context.Publishers.Find(_ => true).ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(string id)
    {
        return await context.Publishers.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Publisher?> GetByNameAsync(string name)
    {
        return await context.Publishers.Find(p => p.Name == name).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Publisher publisher)
    {
        await context.Publishers.InsertOneAsync(publisher);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Publishers.DeleteOneAsync(p => p.Id == id);
    }
}