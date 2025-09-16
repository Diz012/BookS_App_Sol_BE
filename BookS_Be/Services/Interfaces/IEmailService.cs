namespace BookS_Be.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string to, string token);
}