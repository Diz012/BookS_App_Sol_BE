using BookS_Be.DTOs;
using BookS_Be.Models;

namespace BookS_Be.Services.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(string id);
    Task CreateAsync(AddBookDto bookDto, IFormFile file);
    Task UpdateAsync(string id, Book book);
    Task DeleteAsync(string id);
}