using Business.Models.Dtos;
using FluentValidation;

namespace API.Validators;

public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
{
    public ReservationRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("The start date of your reservation is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("The start date of your reservation cannot be in the past.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(24))
            .WithMessage("The start date of your reservation must be within the next 24 hours.");

        RuleFor(x => x.DurationInMinutes)
            .InclusiveBetween(1, 120)
            .WithMessage("The duration of your reservation must be between 1 and 120 minutes.");
    }
}
