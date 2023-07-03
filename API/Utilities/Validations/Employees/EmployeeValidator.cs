using API.Contracts;
using API.DTOs.Employees;
using API.Utilities.Enums;
using FluentValidation;

namespace API.Utilities.Validations.Employees;

public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.NIK)
           .NotEmpty();

        RuleFor(p => p.FirstName)
           .NotEmpty();

        RuleFor(p => p.BirthDate)
           .NotEmpty()
           .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

        RuleFor(p => p.Gender)
           .NotEmpty()
           .Equal(GenderEnum.Female)
           .Equal(GenderEnum.Male);

        RuleFor(p => p.HiringDate)
           .NotEmpty();

        RuleFor(p => p.Email)
           .NotEmpty()
           .EmailAddress();

        RuleFor(p => p.PhoneNumber)
           .NotEmpty()
           .Matches(@"^\+[1-9]\d{1,20}$").WithMessage("Invalid phone number format.");
    }
}