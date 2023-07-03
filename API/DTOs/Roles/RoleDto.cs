using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Roles;

public class RoleDto
{
    public Guid Guid { get; set; }

    [Required]
    public string Name { get; set; }
}