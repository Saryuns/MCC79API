using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/universities")]
public class UniversityController : GeneralController<IUniversityRepository, University>
{
    public UniversityController(IUniversityRepository repository) : base(repository)
    {

    }
    [HttpGet("Name")]
    public IActionResult GetByName(string name)
    {
        var universities = _repository.GetByName(name);
        if (universities.Any())
        {
            return NotFound();
        }

        return Ok(universities);
    }
}