using BookS_Be.Models;
using BookS_Be.Repositories.Interfaces;
using BookS_Be.Services.Interfaces;
using BookS_Be.Helpers;
using MongoDB.Bson;

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
        // Generate unique ID if not provided
        if (string.IsNullOrEmpty(user.Id))
        {
            user.Id = ObjectId.GenerateNewId().ToString();
        }
        
        // Hash the password before storing
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
        }
        
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