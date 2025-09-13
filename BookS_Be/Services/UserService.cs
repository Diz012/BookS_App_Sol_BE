using BookS_Be.Data;
using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;

namespace BookS_Be.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<User>> GetUsersAsync()
    {
        return await userRepository.GetAllAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetByEmailAsync(email);
    }

    public async Task CreateUserAsync(User user)
    {
        await userRepository.CreateAsync(user);
    }

    public async Task UpdateUserAsync(string id, User user)
    {
        await userRepository.UpdateAsync(id, user);
    }

    public async Task DeleteUserAsync(string id)
    {
        await userRepository.DeleteAsync(id);
    }
}