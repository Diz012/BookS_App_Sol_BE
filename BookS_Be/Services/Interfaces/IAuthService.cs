namespace BookS_Be.Services.Interfaces;

public interface IAuthService
{
    Task<bool> ConfirmEmailAsync(string token);
}