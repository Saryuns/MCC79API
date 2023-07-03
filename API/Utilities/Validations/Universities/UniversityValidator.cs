using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities;

public class UniversityValidator : AbstractValidator<UniversityDto>
{
    public UniversityValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.Code)
           .NotEmpty();

        RuleFor(p => p.Name)
           .NotEmpty();
    }
}