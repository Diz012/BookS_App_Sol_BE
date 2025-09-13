using BookS_Be.Models;
using MongoDB.Driver;

namespace BookS_Be.Data;

public class AppDbContext(IMongoDatabase database)
{
    private IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return database.GetCollection<T>(collectionName);
    }
    
    public IMongoCollection<User> Users => GetCollection<User>("users");
}