using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(string id)
    {
        return await bookRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Book book)
    {
        await bookRepository.CreateAsync(book); 
    }

    public async Task UpdateAsync(string id, Book book)
    {
        await bookRepository.UpdateAsync(id, book);
    }

    public async Task DeleteAsync(string id)
    {
        await bookRepository.DeleteAsync(id);
    }
}