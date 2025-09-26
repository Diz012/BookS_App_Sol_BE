using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class UserBooks
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; }
    
    [BsonElement("bookId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string BookId { get; set; }
}