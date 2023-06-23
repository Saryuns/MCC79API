using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/accountroles")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _repository;

    public AccountRoleController(IAccountRoleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accountroles = _repository.GetAll();

        if (!accountroles.Any())
        {
            return NotFound();
        }

        return Ok(accountroles);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var accountroles = _repository.GetByGuid(guid);
        if (accountroles is null)
        {
            return NotFound();
        }

        return Ok(accountroles);
    }

    [HttpPost]
    public IActionResult Create(AccountRole accountrole)
    {
        var createdAccountRole = _repository.Create(accountrole);
        return Ok(createdAccountRole);
    }

    [HttpPut]
    public IActionResult Update(AccountRole accountrole)
    {
        var isUpdated = _repository.Update(accountrole);
        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _repository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}