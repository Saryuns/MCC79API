using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/accountroles")]
public class AccountRoleController : GeneralController<AccountRole>
{
    public AccountRoleController(IRepository<AccountRole> repository) : base(repository)
    {
    }
}