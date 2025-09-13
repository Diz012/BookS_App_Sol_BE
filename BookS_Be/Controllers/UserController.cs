using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
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
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    /// <response code="200">Returns the list of users</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        try
        {
            var users = await userService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Get a specific user by ID
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <returns>User details</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }

    }

    /// <summary>
    /// Get a user by email address
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <returns>User details</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        try
        {
            var user = await userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user">User object to create</param>
    /// <returns>Created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">The user ID to update</param>
    /// <param name="user">Updated user data</param>
    /// <returns>No content</returns>
    /// <response code="204">If the user was updated successfully</response>
    /// <response code="400">If the user data is invalid</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] User user)
    {
        try
        {
            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            await userService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">The user ID to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the user was deleted successfully</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            await userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
