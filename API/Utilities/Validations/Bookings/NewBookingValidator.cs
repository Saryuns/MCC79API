using API.DTOs.Bookings;
using FluentValidation;

namespace API.Utilities.Validations.Bookings;

public class NewBookingValidator : AbstractValidator<NewBookingDto>
{
    public NewBookingValidator()
    {
        RuleFor(p => p.StartDate)
           .GreaterThanOrEqualTo(DateTime.Now.AddMinutes(-60));

        RuleFor(p => p.EndDate)
           .LessThanOrEqualTo(DateTime.Now);

        RuleFor(p => p.Remarks)
          .NotEmpty();

        RuleFor(p => p.EmployeeGuid)
           .NotEmpty();

        RuleFor(p => p.RoomGuid)
           .NotEmpty();
    }
}