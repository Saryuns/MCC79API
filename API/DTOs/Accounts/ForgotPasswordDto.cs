using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}