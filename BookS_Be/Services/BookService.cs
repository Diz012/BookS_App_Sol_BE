using BookS_Be.DTOs;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;
using MongoDB.Bson;

namespace BookS_Be.Services;

public class BookService(IBookRepository bookRepository, ISupabaseService supabaseService) : IBookService
{
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(string id)
    {
        return await bookRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(AddBookDto bookDto, IFormFile file)
    {
        var newId = ObjectId.GenerateNewId().ToString();

        var book = new Book
        {
            Id = newId,
            Title = bookDto.Title,
            AuthorId = bookDto.AuthorId,
            PublisherId = bookDto.PublisherId,
            Isbn = bookDto.Isbn,
            Price = bookDto.Price,
            Stock = bookDto.Stock,
            CategoryIds = bookDto.CategoryIds,
            Description = bookDto.Description,
            PublishedDate = bookDto.PublishedDate
        };

        if (file.Length > 0)
        {
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension)) extension = ".bin";
            var objectPath = $"{newId}/{Guid.NewGuid()}{extension}";
            const string bucket = "book_cover";

            await using var stream = file.OpenReadStream();
            await supabaseService.UploadAsync(bucket, objectPath, stream, file.ContentType);

            var publicUrl = supabaseService.GetPublicUrl(bucket, objectPath);
            book.CoverImageUrl = string.IsNullOrWhiteSpace(publicUrl)
                ? await supabaseService.CreatePresignedUrlAsync(bucket, objectPath)
                : publicUrl;
        }

        await bookRepository.CreateAsync(book);

        if (!string.IsNullOrWhiteSpace(book.CoverImageUrl))
        {
            await bookRepository.UpdateAsync(book.Id, book);
        }
    }

    public async Task UpdateAsync(string id, Book book)
    {
        await bookRepository.UpdateAsync(id, book);
    }

    public async Task DeleteAsync(string id)
    {
        await bookRepository.DeleteAsync(id);
    }
    
    public async Task AddFavoriteBookAsync(string userId, string bookId)
    {
        await bookRepository.AddFavoriteAsync(userId, bookId);
    }
    
    public async Task RemoveFavoriteBookAsync(string userId, string bookId)
    {
        await bookRepository.RemoveFavoriteAsync(userId, bookId);
    }
    
    public async Task<List<Book>> GetUserFavoriteBooksAsync(string userId)
    {
        return await bookRepository.GetUserFavoritesAsync(userId);
    }
}