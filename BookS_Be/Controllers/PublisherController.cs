using BookS_Be.DTOs;
using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublisherController(IPublisherService publisherService) : ControllerBase
{
    /// <summary>
    /// Get all publishers.
    /// </summary>
    /// <remarks>
    /// Returns a list of all publishers in the system.
    /// </remarks>
    /// <response code="200">Returns the list of publishers</response>
    /// <response code="404">If no publishers are found</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(List<Publisher>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPublishers()
    {
        try
        {
            var publishers = await publisherService.GetAllPublishersAsync();
            
            if(publishers.Count == 0)
            {
                return NotFound(new {message = "No publishers found."});
            }
            
            return Ok(publishers);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Add a new publisher.
    /// </summary>
    /// <remarks>
    /// Adds a new publisher to the system.
    /// </remarks>
    /// <param name="publisherDto"></param>
    /// <response code="201">Publisher added successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]    
    public async Task<IActionResult> CreateAsync([FromBody]AddPublisherDto publisherDto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var publisher = new Publisher()
            {
                Name = publisherDto.Name,
                Address = publisherDto.Address,
                Contact = publisherDto.Contact,
            };
            
            await publisherService.AddPublisherAsync(publisher);
            
            return StatusCode(201, new {message = "Publisher added successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Delete a publisher by ID.
    /// </summary>
    /// <remarks>
    /// Deletes a publisher from the system using its ID.
    /// </remarks>
    /// <param name="publisherId">The ID of the publisher to delete.</param>
    /// <response code="200">Publisher deleted successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("[action]/{publisherId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync([FromRoute]string publisherId)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await publisherService.DeletePublisherAsync(publisherId);
            
            return Ok(new {message = "Publisher deleted successfully."});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}