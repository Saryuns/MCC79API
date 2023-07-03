using API.DTOs.Bookings;
using API.Utilities.Enums;
using FluentValidation;

namespace API.Utilities.Validations.Bookings;

public class BookingValidator : AbstractValidator<BookingDto>
{
    public BookingValidator()
    {
        RuleFor(p => p.Guid)
           .NotEmpty();

        RuleFor(p => p.StartDate)
           .GreaterThanOrEqualTo(DateTime.Now.AddMinutes(-60));

        RuleFor(p => p.EndDate)
           .LessThanOrEqualTo(DateTime.Now);

        RuleFor(p => p.Remarks)
          .NotEmpty();

        RuleFor(p => p.Status)
           .NotNull()
           .IsInEnum();

        RuleFor(p => p.EmployeeGuid)
           .NotEmpty();

        RuleFor(p => p.RoomGuid)
           .NotEmpty();
    }
}