using System.Net;
using API.DTOs.Accounts;
using API.Services;
using API.Utilities.Enums;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize(Roles = $"{nameof(RoleType.Admin)}")]

public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterDto register)
    {
        var createdRegister = _service.Register(register);
        if (createdRegister == null)
        {
            return BadRequest(new ResponseHandler<RegisterDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Register failed"
            });
        }

        return Ok(new ResponseHandler<RegisterDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully register",
            Data = createdRegister
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginDto loginDto)
    {
        var login = _service.Login(loginDto);
        if (login is "-1")
        {
            return NotFound(new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account not found"
            });
        }
        if (login is "0")
        {
            return BadRequest(new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Password is incorrect"
            });
        }
        if (login is "-2")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving when creating token"
            });
        }

        return Ok(new ResponseHandler<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success",
            Data = login
        });
    }

    [Authorize(Roles = $"{nameof(RoleType.User)}")]
    [HttpPost("change-password")]
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var updatePassword = _service.ChangePassword(changePasswordDto);
        if (updatePassword == 0)
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not found"
            });

        if (updatePassword == -1)
        {
            return BadRequest(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Otp is already used"
            });
        }

        if (updatePassword == -2)
        {
            return BadRequest(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Otp is incorrect"
            });
        }

        if (updatePassword == -3)
        {
            return BadRequest(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Otp is expired"
            });
        }

        if (updatePassword is -4)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<ChangePasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Password has been changed successfully"
        });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public IActionResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var forgotPassword = _service.ForgotPassword(forgotPasswordDto);
        if (forgotPassword == 0)
            return NotFound(new ResponseHandler<ForgotPasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not found"
            });

        if (forgotPassword is -1)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<ForgotPasswordDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<ForgotPasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Otp has been sent to your email"
        });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetAccount();

        if (entities == null)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<AccountDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _service.GetAccount(guid);
        if (account is null)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = account
        });
    }

    [HttpPost]
    public IActionResult Create(AccountDto newAccountDto)
    {
        var createAccount = _service.CreateAccount(newAccountDto);
        if (createAccount is null)
        {
            return BadRequest(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data Not Created"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Created",
            Data = createAccount
        });
    }

    [HttpPut]
    public IActionResult Update(AccountDto updateAccountDto)
    {
        var update = _service.UpdateAccount(updateAccountDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (update is 0)
        {
            return BadRequest(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Your Data"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteAccount(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "ID Not Found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check Connection to Database"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully Deleted"
        });
    }
}