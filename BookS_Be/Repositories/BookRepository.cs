using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using MongoDB.Driver;

namespace BookS_Be.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    public async Task<List<Book>> GetAllAsync()
    {
        return await context.Books.Find(_ => true).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(string id)
    {
        return await context.Books.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Book>> GetByTitleAsync(string title)
    {
        return await context.Books.Find(b => b.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
    }

    public async Task<List<Book>> GetByAuthorIdAsync(string authorId)
    {
        return await context.Books.Find(b => b.AuthorId == authorId).ToListAsync();
    }

    public async Task<List<Book>> GetByCategoryIdAsync(string categoryId)
    {
        return await context.Books.Find(b => b.CategoryIds.Contains(categoryId)).ToListAsync();
    }

    public async Task<List<Book>> GetByPublisherIdAsync(string publisherId)
    {
        return await context.Books.Find(b => b.PublisherId == publisherId).ToListAsync();
    }

    public async Task CreateAsync(Book book)
    {
        await context.Books.InsertOneAsync(book);
    }

    public async Task UpdateAsync(string id, Book book)
    {
        await context.Books.ReplaceOneAsync(b => b.Id == id, book);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Books.DeleteOneAsync(b => b.Id == id);
    }
}