using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/accountroles")]
public class AccountRoleController : GeneralController<IAccountRoleRepository, AccountRole>
{
    public AccountRoleController(IAccountRoleRepository repository) : base(repository)
    {
    }
}