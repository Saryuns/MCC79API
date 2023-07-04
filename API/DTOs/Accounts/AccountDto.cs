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
}