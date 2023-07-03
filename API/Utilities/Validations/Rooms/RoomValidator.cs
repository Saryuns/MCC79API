using API.DTOs.Rooms;
using FluentValidation;

namespace API.Utilities.Validations.Rooms;

public class RoomValidator : AbstractValidator<RoomDto>
{
    public RoomValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.Name)
           .NotEmpty();

        RuleFor(p => p.Capacity)
           .NotEmpty();

        RuleFor(p => p.Floor)
           .NotEmpty();
    }
}