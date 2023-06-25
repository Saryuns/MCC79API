using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/universities")]
public class UniversityController : GeneralController<University>
{
    public UniversityController(IRepository<University> repository) : base(repository)
    {

    }
}