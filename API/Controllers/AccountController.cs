using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : GeneralController<IAccountRepository, Account>
{
    public AccountController(IAccountRepository repository) : base(repository)
    {
    }
}