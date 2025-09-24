using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.DTOs;

public class AddBookWithFileDto
{
    [Required (ErrorMessage = "Cover image file is required")]
    [FromForm(Name = "CoverImageFile")]
    public required IFormFile CoverImageFile { get; set; }
    [Required(ErrorMessage = "Meta data is required")]
    [FromForm(Name = "MetaData")]
    [DefaultValue("\"{\\\"title\\\":\\\"Sapiens\\\",\\\"authorId\\\":\\\"68cd065c8a14662fec49aba0\\\",\\\"publisherId\\\":\\\"68cd106ae0797f915aa2dc23\\\",\\\"isbn\\\":\\\"0099590085\\\",\\\"price\\\":23,\\\"stock\\\":20,\\\"categoryIds\\\":[\\\"68ccff1be27cf2ea2be73729\\\",\\\"68cd19af953501defdaea182\\\"],\\\"description\\\":\\\"How did our species succeed in the battle for dominance Why did our foraging ancestors come together to create cities and kingdoms How did we come to believe in gods nations and human rights to trust money books and laws and to be enslaved by bureaucracy timetables and consumerism And what will our world be like in the millennia to come\\\",\\\"publishedDate\\\":\\\"2025-09-19T08:52:21.629Z\\\"}\"")]
    public required string MetaData { get; set; }
}

