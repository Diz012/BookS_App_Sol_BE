using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookS_Be.DTOs;
using BookS_Be.Helpers;
using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookController(IBookService bookService, JwtHelper jwtHelper) : ControllerBase
{
    /// <summary>
    /// Get all books.
    /// </summary>
    /// <remarks>
    /// Returns a list of all books in the system.
    /// </remarks>
    /// <response code="200">Returns the list of books</response>
    /// <response code="404">If no books are found</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var books = await bookService.GetAllBooksAsync();

            if (books.Count == 0)
            {
                return NotFound(new {message = "No books found."});
            }
            
            return Ok(books);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Add a new book.
    /// </summary>
    /// <remarks>
    /// Adds a new book to the system.
    /// </remarks>
    /// <param name="dto"></param>
    /// <response code="201">Book added successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("[action]")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBook([FromForm] AddBookWithFileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var bookDto = System.Text.Json.JsonSerializer.Deserialize<AddBookDto>(
                dto.MetaData,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            
            if (bookDto == null)
                return BadRequest(new {message = "Invalid book data."});
            
            await bookService.CreateAsync(bookDto, dto.CoverImageFile);
            
            return StatusCode(201, new {message = "Book added successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Add a book to user's favorite list.
    /// </summary>
    /// <remarks>
    /// Adds a book to the authenticated user's favorite list.
    /// </remarks>
    /// <param name="bookId">The ID of the book to add to favorites.</param>
    /// <response code="201">Book added to favorites successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("[action]/{bookId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFavoriteBook([FromRoute] string bookId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header." });
            }
            
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = jwtHelper.ValidateToken(token);
            
            if (principal == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }
            
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            
            await bookService.AddFavoriteBookAsync(userId, bookId);
         
            return StatusCode(201, new {message = "Book added successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Remove a book from user's favorite list.
    /// </summary>
    /// <remarks>
    /// Removes a book from the authenticated user's favorite list.
    /// </remarks>
    /// <param name="bookId">The ID of the book to remove from favorites.</param>
    /// <response code="201">Book removed from favorites successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("[action]/{bookId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFavoriteBook([FromRoute] string bookId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header." });
            }
            
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = jwtHelper.ValidateToken(token);
            
            if (principal == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }
            
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            
            await bookService.RemoveFavoriteBookAsync(userId, bookId);
         
            return StatusCode(201, new {message = "Book removed successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
