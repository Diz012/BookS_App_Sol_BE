using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class Author
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("name")]
    public required string Name { get; set; }
    
    [BsonElement("bio")]
    public string? Bio { get; set; } = string.Empty;
    
    [BsonElement("birthDate")]
    public DateTime? BirthDate { get; set; }
}