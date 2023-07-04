using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts;

public class ChangePasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public int OTP { get; set; }

    [Required]
    [PasswordPolicy]
    public string NewPassword { get; set; }

    [Required]
    [PasswordPolicy]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }
}
