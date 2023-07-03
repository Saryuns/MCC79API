using API.Models;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts;

public class AccountDto
{
    [Required]
    public Guid Guid { get; set; }

    [Required]
    [PasswordPolicy]
    public string Password { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [Required]
    public int OTP { get; set; }

    [Required]
    public bool IsUsed { get; set; }

    [Required]
    public DateTime ExpiredTime { get; set; }

    public List<RoleType> Roles { get; set; }
}

    /* add
    public static implicit operator Account(AccountDto accountDto)
    {
        return new()
        {
            Guid = accountDto.Guid,
            Password = accountDto.Password,
            IsDeleted = accountDto.IsDeleted,
            OTP = accountDto.OTP,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime
        };
    }

    public static explicit operator AccountDto(Account account)
    {
        return new()
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }
    */