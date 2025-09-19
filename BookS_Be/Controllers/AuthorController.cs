using BookS_Be.DTOs;
using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    /// <summary>
    /// Get all authors.
    /// </summary>
    /// <remarks>
    /// Returns a list of all authors in the system.
    /// </remarks>
    /// <response code="200">Returns the list of authors</response>
    /// <response code="404">If no authors are found</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(List<Author>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthors()
    {
        try
        {
            var authors = await authorService.GetAllAuthorsAsync();
            
            if(authors.Count == 0)
            {
                return NotFound(new {message = "No authors found."});
            }
            
            return Ok(authors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Add a new author.
    /// </summary>
    /// <remarks>
    /// Adds a new author to the system.
    /// </remarks>
    /// <param name="authorDto">Author data</param>
    /// <response code="201">Author added successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAuthor([FromBody] AddAuthorDto authorDto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = new Author()
            {
                Name = authorDto.Name,
                Bio = authorDto.Bio
            };
            await authorService.AddAuthorAsync(author);
            
            return StatusCode(201, new {message = "Author added successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Delete an author by ID.
    /// </summary>
    /// <remarks>
    /// Deletes an author from the system by their ID.
    /// </remarks>
    /// <param name="authorId">Author ID</param>
    /// <response code="204">Author deleted successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("[action]/{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor([FromRoute] string authorId)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await authorService.DeleteAuthorAsync(authorId);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}

