using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BookS_Be.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found in user secrets");
        _issuer = _configuration["JWT:Issuer"] ?? "BookS-API";
        _audience = _configuration["JWT:Audience"] ?? "BookS-Client";
        _expirationHours = int.Parse(_configuration["JWT:ExpirationHours"] ?? "24");
    }

    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="email">User email</param>
    /// <returns>JWT token string</returns>
    public string GenerateToken(string userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, 
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                ClaimValueTypes.Integer64)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_expirationHours),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token and returns the claims principal
    /// </summary>
    /// <param name="token">JWT token to validate</param>
    /// <returns>ClaimsPrincipal if valid, null if invalid</returns>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extracts the user ID from a JWT token without validating it
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>User ID if found, null otherwise</returns>
    public static string? GetUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extracts the email from a JWT token without validating it
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Email if found, null otherwise</returns>
    public static string? GetEmailFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if a JWT token is expired
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>True if expired, false otherwise</returns>
    public static bool IsTokenExpired(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// Gets the expiration time of a JWT token
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Expiration DateTime if found, null otherwise</returns>
    public static DateTime? GetTokenExpiration(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.ValidTo;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extracts all claims from a JWT token without validating it
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Dictionary of claims if successful, empty dictionary otherwise</returns>
    public static Dictionary<string, string> GetClaimsFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }
}
