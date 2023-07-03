using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles;

public class AccountRoleValidator : AbstractValidator<AccountRoleDto>
{
    public AccountRoleValidator()
    {
        RuleFor(p => p.AccountGuid)
           .NotEmpty();

        RuleFor(p => p.RoleGuid)
           .NotEmpty();
    }
}