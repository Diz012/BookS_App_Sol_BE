using System.ComponentModel.DataAnnotations;

namespace BookS_Be.DTOs;

public class AddBookDto
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(10, ErrorMessage = "Title must be at least 2 characters long")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public required string Title { get; set; }
    
    [Required(ErrorMessage = "AuthorId is required")]
    public required string AuthorId { get; set; }
    
    [Required(ErrorMessage = "PublisherId is required")]
    public required string PublisherId { get; set; }
    
    public string Isbn { get; set; } = string.Empty;
    
    public float Price { get; set; } = 0.0f;

    public int Stock { get; set; } = 0;
    
    [Required(ErrorMessage = "At least one CategoryId is required")]
    public required string[] CategoryIds { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
}