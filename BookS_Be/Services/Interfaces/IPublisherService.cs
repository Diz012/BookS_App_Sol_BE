using BookS_Be.Models;

namespace BookS_Be.Services.Interfaces;

public interface IPublisherService
{
    Task<List<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetPublisherByIdAsync(string id);
    Task<Publisher?> GetPublisherByNameAsync(string name);
    Task AddPublisherAsync(Publisher publisher);
    Task DeletePublisherAsync(string publisherId);
}