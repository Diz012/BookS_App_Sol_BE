using System.ComponentModel.DataAnnotations;

namespace BookS_Be.DTOs;

public class AddAuthorDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(10, ErrorMessage = "Name must be at least 10 characters long")]
    public required string Name { get; set; }
    
    [MaxLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters")]
    [MinLength(20, ErrorMessage = "Bio must be at least 20 characters long")]
    public string? Bio { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }
}