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
    public IMongoCollection<Book> Books => GetCollection<Book>("books");
    public IMongoCollection<Author> Authors => GetCollection<Author>("authors");
    public IMongoCollection<Publisher> Publishers => GetCollection<Publisher>("publishers");
    public IMongoCollection<Category> Categories => GetCollection<Category>("categories");
    public IMongoCollection<UserBooks> UserBooks => GetCollection<UserBooks>("userBooks");
}