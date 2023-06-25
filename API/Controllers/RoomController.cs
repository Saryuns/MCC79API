using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : GeneralController<Room>
{
    public RoomController(IRepository<Room> repository) : base(repository)
    {
    }
}