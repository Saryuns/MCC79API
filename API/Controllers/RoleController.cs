using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : GeneralController<IRoleRepository, Role>
{
    public RoleController(IRoleRepository repository) : base(repository)
    {
    }
}