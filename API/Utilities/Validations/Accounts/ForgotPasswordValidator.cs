using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidation()
    {
        RuleFor(p => p.Email)
           .NotEmpty()
           .EmailAddress();
    }
}