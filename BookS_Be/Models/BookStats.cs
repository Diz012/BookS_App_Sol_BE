using MongoDB.Bson.Serialization.Attributes;

namespace BookS_Be.Models;

public class BookStats
{
    [BsonElement("favorites")]
    public int Favorites { get; set; } = 0;
    
    [BsonElement("purchases")]
    public int Purchases { get; set; } = 0; 
}