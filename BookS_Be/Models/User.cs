using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("username")]
    public required string Username { get; set; }
    
    [BsonElement("email")]
    public required string Email { get; set; }
    
    [BsonElement("passwordHash")]
    public required string PasswordHash { get; set; }
    
    [BsonElement("fullName")]
    public string FullName { get; set; } = string.Empty;
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("isEmailVerified")]
    public bool IsEmailVerified { get; set; }
}