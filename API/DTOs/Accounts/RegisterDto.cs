using API.Utilities;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Required 0 or 1. 0 = Female, 1 = Male")]
        public GenderEnum Gender { get; set; }

        [Required]
        public DateTime HiringDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Major { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        [Range(0, 4, ErrorMessage = "GPA must betwen 0 - 4")]
        public Double GPA { get; set; }

        [Required]
        public string UniversityCode { get; set; }

        [Required]
        public string UniversityName { get; set; }

        [Required]
        [PasswordPolicy]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}