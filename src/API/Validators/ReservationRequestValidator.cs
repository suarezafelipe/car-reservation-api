using Business.Models.Dtos;
using FluentValidation;

namespace API.Validators
{
    public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
    {
        public ReservationRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("The start date of your reservation is required.")
                .Must(BeWithinNext24Hours)
                .WithMessage("The start date of your reservation must be within the next 24 hours.");

            RuleFor(x => x.DurationInMinutes)
                .InclusiveBetween(1, 120)
                .WithMessage("The duration of your reservation must be between 1 and 120 minutes.");
        }

        private bool BeWithinNext24Hours(DateTime startDate)
        {
            var now = DateTime.UtcNow;
            var maxAllowedDate = now.AddHours(24);

            return startDate >= now && startDate <= maxAllowedDate;
        }
    }
}
