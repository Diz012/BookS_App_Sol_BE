using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class Publisher
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("name")]
    public required string Name { get; init; }
    
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
    
    [BsonElement("contact")]
    public string Contact { get; set; } = string.Empty;
}