using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : GeneralController<IRoomRepository, Room>
{
    public RoomController(IRoomRepository repository) : base(repository)
    {
    }
}