namespace BookS_Be.Services.Interfaces;

public interface ISupabaseService
{
    Task<string> CreatePresignedUrlAsync(string bucket, string filePath, int expiresInSeconds = 3600);
    Task<string> UploadAsync(string bucket, string filePath, Stream content, string contentType, bool upsert = true);
    string GetPublicUrl(string bucket, string filePath);
}