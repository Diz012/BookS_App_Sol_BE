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
    
    public async Task AddFavoriteAsync(string userId, string bookId)
    {
        var book = await GetByIdAsync(bookId);
        if(book == null) throw new Exception("Book not found");
        
        var exists = await context.UserBooks.Find(ub => ub.UserId == userId && ub.BookId == bookId).AnyAsync();
        if (exists) throw new Exception("Book already in favorites");
        
        await context.UserBooks.InsertOneAsync(new UserBooks { UserId = userId, BookId = bookId });
        book.Stats.Favorites++;
        await UpdateAsync(bookId, book);
    }
    
    public async Task RemoveFavoriteAsync(string userId, string bookId)
    {
        var book = await GetByIdAsync(bookId);
        if(book == null) throw new Exception("Book not found");
        
        var exists = await context.UserBooks.Find(ub => ub.UserId == userId && ub.BookId == bookId).AnyAsync();
        if (exists) throw new Exception("Book not in favorites");
        
        await context.UserBooks.DeleteOneAsync(ub => ub.UserId == userId && ub.BookId == bookId);
        book.Stats.Favorites = Math.Max(0, book.Stats.Favorites--);
        await UpdateAsync(bookId, book);
    }
    
    public async Task<List<Book>> GetUserFavoritesAsync(string userId)
    {
        var favoriteBookIds = await context.UserBooks.Find(ub => ub.UserId == userId).ToListAsync();
        var bookIds = favoriteBookIds.Select(ub => ub.BookId).ToList();
        return await context.Books.Find(b => b.Id != null && bookIds.Contains(b.Id)).ToListAsync();
    }
}