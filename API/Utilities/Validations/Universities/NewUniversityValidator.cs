using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities;

public class NewUniversityValidator : AbstractValidator<NewUniversityDto>
{
    public NewUniversityValidator()
    {
        RuleFor(p => p.Code)
           .NotEmpty();

        RuleFor(p => p.Name)
           .NotEmpty();
    }
}
