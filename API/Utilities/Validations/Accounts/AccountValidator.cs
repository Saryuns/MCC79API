using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class AccountValidator : AbstractValidator<AccountDto>
{
    public AccountValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.Password)
           .NotEmpty().WithMessage("Password is required.")
           .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
           .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
           .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
           .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
           .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(p => p.IsDeleted)
           .NotEmpty();

        RuleFor(p => p.OTP)
           .NotEmpty();

        RuleFor(p => p.IsUsed)
          .NotEmpty();

        RuleFor(p => p.ExpiredTime)
          .NotEmpty();
    }
}