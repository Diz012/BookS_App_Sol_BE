using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{
    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await authorRepository.GetAllAsync();
    }

    public async Task<Author?> GetByIdAsync(string id)
    {
        return await authorRepository.GetByIdAsync(id);
    }

    public async Task AddAuthorAsync(Author author)
    {
        await authorRepository.CreateAsync(author);
    }

    public async Task DeleteAuthorAsync(string authorId)
    {
        await authorRepository.DeleteAsync(authorId);
    }
}