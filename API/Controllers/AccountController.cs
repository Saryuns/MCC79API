using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : GeneralController<Account>
{
    public AccountController(IRepository<Account> repository) : base(repository)
    {
    }
}