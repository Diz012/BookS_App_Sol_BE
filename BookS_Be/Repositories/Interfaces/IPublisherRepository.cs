using BookS_Be.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Repositories.Interfaces;

public interface IPublisherRepository
{
    Task <List<Publisher>> GetAllAsync();
    Task <Publisher?> GetByIdAsync(string id);
    Task <Publisher?> GetByNameAsync(string name);
    Task CreateAsync(Publisher publisher);
    Task DeleteAsync(string id);
}