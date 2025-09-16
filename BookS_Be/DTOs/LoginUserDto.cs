using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookS_Be.DTOs;

/// <summary>
/// Data Transfer Object for user login request
/// </summary>
public class LoginUserDto
{
    /// <summary>
    /// Email address for login
    /// Example: john.doe@example.com
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must be a valid email address")]
    [DefaultValue("username@example.com")]
    public required string Email { get; set; }
    
    /// <summary>
    /// Password for login
    /// Example: MySecure123!
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [DefaultValue("P@ssw0rd!")]
    public required string Password { get; set; }
}
