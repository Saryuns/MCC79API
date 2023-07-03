using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Rooms;

public class RoomDto
{
    public Guid Guid { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Floor { get; set; }

    [Required]
    public int Capacity { get; set; }
}