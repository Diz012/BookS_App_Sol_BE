using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BookS_Be.Services;

public class SupabaseService() : ISupabaseService
{
    private readonly Supabase.Client _client;
    private readonly SupabaseSettings _settings;
    
    public SupabaseService(IOptions<SupabaseSettings> options) : this()
    {
        _settings = options.Value;
        _client = new Supabase.Client(_settings.Url, _settings.ServiceRoleKey);
        _client.InitializeAsync().Wait();
    }
    
    public async Task<string> CreatePresignedUrlAsync(string bucket, string filePath, int expiresInSeconds = 3600)
    {
        var storage = _client.Storage;
        var bucketRef = storage.From(bucket);
        
        var signedUrl = await bucketRef.CreateSignedUrl(filePath, expiresInSeconds);
        
        return signedUrl;
    }

    public async Task<string> UploadAsync(string bucket, string filePath, Stream content, string contentType, bool upsert = true)
    {
        var storage = _client.Storage;
    
        // Check if bucket exists
        var buckets = await storage.ListBuckets();
        if (buckets != null && buckets.All(b => b.Name != bucket))
        {
            await storage.CreateBucket(bucket);
        }
    
        var bucketRef = storage.From(bucket);
        var options = new Supabase.Storage.FileOptions
        {
            CacheControl = "3600",
            ContentType = contentType,
            Upsert = upsert
        };
        using var memoryStream = new MemoryStream();
        await content.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        await bucketRef.Upload(fileBytes, filePath, options);
    
        return filePath;
    }

    public string GetPublicUrl(string bucket, string filePath)
    {
        var storage = _client.Storage;
        var bucketRef = storage.From(bucket);
        return bucketRef.GetPublicUrl(filePath);
    }
}