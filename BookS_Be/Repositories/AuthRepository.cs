using BookS_Be.Repositories.Interfaces;

namespace BookS_Be.Repositories;

public class AuthRepository(IUserRepository userRepository) : IAuthRepository
{
    public async Task SetEmailConfirmedAsync(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.IsEmailVerified = true;
        await userRepository.UpdateAsync(user.Id ?? throw new InvalidOperationException(), user);
    }
}