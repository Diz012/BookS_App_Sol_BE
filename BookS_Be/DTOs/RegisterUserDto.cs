using System.ComponentModel.DataAnnotations;

namespace BookS_Be.DTOs;

/// <summary>
/// Data Transfer Object for user registration request
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// Username for the user (required)
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public required string Username { get; set; }
    
    /// <summary>
    /// Email address for the user (required)
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
    
    /// <summary>
    /// Password for the user (required)
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public required string Password { get; set; }
    
    /// <summary>
    /// Full name of the user (optional)
    /// </summary>
    [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
    public string? FullName { get; set; }
}
