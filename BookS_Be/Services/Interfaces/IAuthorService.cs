using BookS_Be.Models;

namespace BookS_Be.Services.Interfaces;

public interface IAuthorService
{
    Task <List<Author>> GetAllAuthorsAsync();
    Task <Author?> GetByIdAsync(string id);
    Task AddAuthorAsync(Author author);
    Task DeleteAuthorAsync(string authorId);
}