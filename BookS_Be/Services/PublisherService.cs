using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class PublisherService(IPublisherRepository publisherRepository) : IPublisherService
{
    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        return await publisherRepository.GetAllAsync();
    }

    public async Task<Publisher?> GetPublisherByIdAsync(string id)
    {
        return await publisherRepository.GetByIdAsync(id);
    }

    public async Task<Publisher?> GetPublisherByNameAsync(string name)
    {
        return await publisherRepository.GetByNameAsync(name);
    }

    public async Task AddPublisherAsync(Publisher publisher)
    {
        await publisherRepository.CreateAsync(publisher);
    }

    public async Task DeletePublisherAsync(string publisherId)
    {
        await publisherRepository.DeleteAsync(publisherId);
    }
}