using BookS_Be.DTOs;
using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;

namespace BookS_Be.Controllers;
 
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
 /// <summary>
 /// Get all categories.
 /// </summary>
 /// <remarks>
 /// Returns a list of all categories in the system.
 /// </remarks>
 /// <response code="200">Returns the list of categories</response>
 /// <response code="404">If no categories are found</response>
 /// <response code="400">If the request is invalid</response>
 /// <response code="500">If an internal server error occurs</response>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var categories = await categoryService.GetCategoriesAsync();
            
            if (categories.Count == 0)
            {
                return NotFound(new { message = "No categories found" });
            }
            return Ok(categories);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Add a new category.
    /// </summary>
    /// <remarks>
    /// Adds a new category to the system.
    /// </remarks>
    /// <param name="categoryDto"></param>
    /// <response code="201">Category added successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto categoryDto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category()
            {
                Description = categoryDto.Description,
                Name = categoryDto.Name
            };
            
            await categoryService.AddCategoryAsync(category);
            
            return StatusCode(201, new { message = "Category added successfully" });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Delete a category by its ID.
    /// </summary>
    /// <remarks>
    /// Deletes the category with the specified ID from the system.
    /// </remarks>
    /// <param name="categoryId">The ID of the category to delete.</param>
    /// <response code="200">Category deleted successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("[action]/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory([FromRoute]string categoryId)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            await categoryService.DeleteCategoryAsync(categoryId);
            
            return Ok(new { message = "Category deleted successfully" });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}