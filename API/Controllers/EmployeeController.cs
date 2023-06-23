using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _repository.GetAll();

        if (!employees.Any())
        {
            return NotFound();
        }

        return Ok(employees);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _repository.GetByGuid(guid);
        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        var createdEmployee = _repository.Create(employee);
        return Ok(createdEmployee);
    }

    [HttpPut]
    public IActionResult Update(Employee employee)
    {
        var isUpdated = _repository.Update(employee);
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