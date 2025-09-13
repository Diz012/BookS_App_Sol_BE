using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using MongoDB.Driver;

namespace BookS_Be.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users.Find(_ => true).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await context.Users.Find(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(User user)
    {
        await context.Users.InsertOneAsync(user);
    }

    public async Task UpdateAsync(string id, User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        await context.Users.ReplaceOneAsync(u => u.Id == id, user);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Users.DeleteOneAsync(x => x.Id == id);
    }
}