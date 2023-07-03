using API.DTOs.Educations;
using FluentValidation;

namespace API.Utilities.Validations.Educations;

public class EducationValidator : AbstractValidator<EducationDto>
{
    public EducationValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.Major)
           .NotEmpty();

        RuleFor(p => p.Degree)
           .NotEmpty()
           .Must(BeValidDegreeLevel)
           .WithMessage("Invalid Degree. Valid values are Diploma, Bachelor, or Master.");

        RuleFor(p => p.GPA)
           .NotEmpty()
           .InclusiveBetween(0, 4).WithMessage("GPA must be between 0 and 4.");

        RuleFor(p => p.UniversityGuid)
           .NotEmpty();
    }

    private bool BeValidDegreeLevel(string degreeLevel)
    {
        var validValues = new[] { "Diploma", "Bachelor", "Master", "Magister" };
        return validValues.Contains(degreeLevel);
    }
}