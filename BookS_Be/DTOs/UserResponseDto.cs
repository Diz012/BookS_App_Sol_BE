namespace BookS_Be.DTOs;

/// <summary>
/// Data Transfer Object for user response
/// </summary>
public class UserResponseDto
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// User's username
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// User's email address
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// User's full name
    /// </summary>
    public string? FullName { get; set; }
    
    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the user was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Response DTO for authentication operations
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public required string Token { get; set; }
    
    /// <summary>
    /// User information
    /// </summary>
    public required UserResponseDto User { get; set; }
    
    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}
