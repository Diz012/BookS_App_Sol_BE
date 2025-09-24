namespace BookS_Be.DTOs;

public class AddPublisherDto
{
    public required string Name { get; set; }
    
    public string Address { get; set; } = string.Empty;
    
    public string Contact { get; set; } = string.Empty;
}