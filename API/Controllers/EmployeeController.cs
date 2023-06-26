using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : GeneralController<IEmployeeRepository, Employee>
{
    public EmployeeController(IEmployeeRepository repository) : base(repository)
    {
    }
}