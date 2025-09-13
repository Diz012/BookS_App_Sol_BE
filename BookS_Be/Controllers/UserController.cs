using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using BookS_Be.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.Controllers;

/// <summary>
/// User management controller
/// </summary>
[ApiController]
[Route("api/user")]
[Produces("application/json")]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registerDto">User registration data</param>
    /// <returns>Created user information</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="409">If user with email already exists</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await userService.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "User with this email already exists" });
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password,
                FullName = registerDto.FullName ?? string.Empty
            };

            await userService.CreateUserAsync(user);

            var userResponse = new UserResponseDto
            {
                Id = user.Id!,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return StatusCode(201, userResponse);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user", details = e.Message });
        }
    }

}
