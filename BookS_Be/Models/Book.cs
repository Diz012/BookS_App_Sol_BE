using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("title")]
    public required string Title { get; set; }
    
    [BsonElement("authorId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string AuthorId { get; set; }
    
    [BsonElement("publisherId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string PublisherId { get; set; }
    
    [BsonElement("isbn")]
    public string Isbn { get; set; } = string.Empty;
    
    [BsonElement("price")]
    public float Price { get; set; }
    
    [BsonElement("stock")]
    public int Stock { get; set; }
    
    [BsonElement("categoryIds")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string[] CategoryIds { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
    
    [BsonElement("coverImageUrl")]
    public string CoverImageUrl { get; set; } = string.Empty;
    
    [BsonElement("publishedDate")]
    public DateTime PublishedDate { get; set; }
}