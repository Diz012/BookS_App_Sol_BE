using BCrypt.Net;

namespace BookS_Be.Helpers;

public static class PasswordHelper
{
    /// <summary>
    /// Hash a plain text password using BCrypt
    /// </summary>
    /// <param name="password">The plain text password to hash</param>
    /// <returns>The hashed password</returns>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
            
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    /// <summary>
    /// Verify a plain text password against a hashed password
    /// </summary>
    /// <param name="password">The plain text password to verify</param>
    /// <param name="hashedPassword">The hashed password to verify against</param>
    /// <returns>True if password matches, false otherwise</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            return false;
            
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    
    /// <summary>
    /// Generate a strong random password
    /// </summary>
    /// <param name="length">Length of the password (default: 12)</param>
    /// <returns>A randomly generated password</returns>
    public static string GenerateRandomPassword(int length = 12)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
