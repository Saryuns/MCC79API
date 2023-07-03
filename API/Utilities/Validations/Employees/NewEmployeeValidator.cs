using API.Contracts;
using API.DTOs.Employees;
using API.Utilities.Enums;
using FluentValidation;

namespace API.Utilities.Validations.Employees;

public class NewEmployeeValidator : AbstractValidator<NewEmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public NewEmployeeValidator(IEmployeeRepository employeeRepository)
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
    }

    private bool BeUniqueProperty(string property)
    {
        return _employeeRepository.IsDuplicateValue(property);
    }
}
