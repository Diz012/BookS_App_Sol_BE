using BookS_Be.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task <List<Author>> GetAllAsync();
    Task <Author?> GetByIdAsync(string id);
    Task <Author?> GetByNameAsync(string name);
    Task CreateAsync(Author author);
    Task DeleteAsync(string id);
}