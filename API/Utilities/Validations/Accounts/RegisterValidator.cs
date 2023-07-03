using API.Contracts;
using API.DTOs.Accounts;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Utilities.Validations.Accounts;

public class RegisterValidation : AbstractValidator<RegisterDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public RegisterValidation(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(p => p.FirstName)
           .NotEmpty();

        RuleFor(p => p.BirthDate)
           .NotEmpty()
           .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

        RuleFor(p => p.Gender)
           .NotNull()
           .IsInEnum();

        RuleFor(p => p.HiringDate)
           .NotEmpty();

        RuleFor(p => p.Email)
           .NotEmpty()
           .Must(BeUniqueProperty).WithMessage("'Email' already registered")
           .EmailAddress();

        RuleFor(p => p.PhoneNumber)
           .NotEmpty()
           .Must(BeUniqueProperty).WithMessage("'Phone Number' already registered")
           .Matches(@"^\+[1-9]\d{1,20}$").WithMessage("Invalid phone number format.");

        RuleFor(p => p.Major)
           .NotEmpty();

        RuleFor(p => p.Degree)
           .NotEmpty()
           .Must(BeValidDegreeLevel)
           .WithMessage("Invalid Degree. Valid values are Diploma, Bachelor, or Master.");

        RuleFor(p => p.GPA)
           .NotEmpty()
           .InclusiveBetween(0, 4).WithMessage("GPA must be between 0 and 4.");

        RuleFor(p => p.UniversityCode)
          .NotEmpty();

        RuleFor(p => p.UniversityName)
          .NotEmpty();

        RuleFor(p => p.Password)
           .NotEmpty().WithMessage("Password is required.")
           .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
           .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
           .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
           .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
           .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(p => p.ConfirmPassword)
          .NotEmpty()
          .Equal(model => model.Password).WithMessage("Password and Confirm Password do not match.");
    }

    private bool BeUniqueProperty(string property)
    {
        return _employeeRepository.IsDuplicateValue(property);
    }

    private bool BeValidDegreeLevel(string degreeLevel)
    {
        var validValues = new[] { "Diploma", "Bachelor", "Master" };
        return validValues.Contains(degreeLevel);
    }
}
