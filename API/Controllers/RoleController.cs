using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : GeneralController<Role>
{
    public RoleController(IRepository<Role> repository) : base(repository)
    {
    }
}