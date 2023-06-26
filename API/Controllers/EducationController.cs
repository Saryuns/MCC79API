using API.Contracts;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/educations")]
public class EducationController : GeneralController<IEducationRepository, Education>
{
    public EducationController(IEducationRepository repository) : base(repository)
    {
    }
}