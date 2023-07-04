using System.Net;
using API.DTOs.Roles;
using API.Services;
using API.Utilities.Enums;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = $"{nameof(RoleType.Admin)}")]
public class RoleController : ControllerBase
{
    private readonly RoleService _service;

    public RoleController(RoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetRole();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<RoleDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var role = _service.GetRole(guid);
        if (role is null)
        {
            return NotFound(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        return Ok(new ResponseHandler<RoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = role
        });
    }

    [HttpPost]
    public IActionResult Create(NewRoleDto newRoleDto)
    {
        var createRole = _service.CreateRole(newRoleDto);
        if (createRole is null)
        {
            return BadRequest(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data Not Created"
            });
        }

        return Ok(new ResponseHandler<RoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Created",
            Data = createRole
        });
    }

    [HttpPut]
    public IActionResult Update(RoleDto updateRoleDto)
    {
        var update = _service.UpdateRole(updateRoleDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (update is 0)
        {
            return BadRequest(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Your Data"
            });
        }

        return Ok(new ResponseHandler<RoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteRole(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<RoleDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Connection to Database"
            });
        }

        return Ok(new ResponseHandler<RoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Deleted"
        });
    }
}