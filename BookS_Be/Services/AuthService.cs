using System.Security.Claims;
using BookS_Be.Helpers;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class AuthService(IAuthRepository authRepository, JwtHelper jwtHelper) : IAuthService
{
    public async Task<bool> ConfirmEmailAsync(string token)
    {
        var principal = jwtHelper.ValidateEmailToken(token);
        if (principal == null)
        {
            return false;
        }
        var email = principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        
        await authRepository.SetEmailConfirmedAsync(email);
        
        return true;
    }
}