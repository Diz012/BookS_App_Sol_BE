using BookS_Be.Models;

namespace BookS_Be.Repositories.Interfaces;

public interface IBookRepository
{
    Task <List<Book>> GetAllAsync();
    Task <Book?> GetByIdAsync(string id);
    Task <List<Book>> GetByTitleAsync(string title);
    Task <List<Book>> GetByAuthorIdAsync(string authorId);
    Task <List<Book>> GetByCategoryIdAsync(string categoryId);
    Task <List<Book>> GetByPublisherIdAsync(string publisherId);
    Task CreateAsync(Book book);
    Task UpdateAsync(string id, Book book);
    Task DeleteAsync(string id);
}