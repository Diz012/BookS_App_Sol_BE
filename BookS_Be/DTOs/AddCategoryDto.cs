
using System.ComponentModel.DataAnnotations;

namespace BookS_Be.DTOs;

public class AddCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description must be at least 5 characters long")]
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 200 characters")]
    public required string Description { get; set; }
}