using FluentValidation;
using Business.Interfaces;
using Business.Models.Entities;

namespace API.Validators
{
    public class CarValidator : AbstractValidator<Car>
    {
        private readonly ICarService _carService;

        public CarValidator(ICarService carService)
        {
            _carService = carService;

            RuleFor(c => c.Make).NotEmpty().WithMessage("Make is required.");
            RuleFor(c => c.Model).NotEmpty().WithMessage("Model is required.");
            RuleFor(c => c.UniqueIdentifier)
                .NotEmpty().WithMessage("UniqueIdentifier is required.")
                .Matches("^[Cc][0-9]+$").WithMessage("UniqueIdentifier should start with 'C' followed by numbers only.")
                .Must(IsUniqueIdentifierUnique).WithMessage("UniqueIdentifier must be unique.");
        }

        private bool IsUniqueIdentifierUnique(string uniqueIdentifier)
        {
            return _carService.IsUniqueIdentifierUnique(uniqueIdentifier);
        }
    }
}
