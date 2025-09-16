namespace BookS_Be.Repositories.Interfaces;

public interface IAuthRepository
{
    Task SetEmailConfirmedAsync(string email);
}