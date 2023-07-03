using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class NewUniversityDto
{
    public string Code { get; set; }

    [Required]
    public string Name { get; set; }
}