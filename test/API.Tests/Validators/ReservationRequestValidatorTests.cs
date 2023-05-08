using System;
using API.Validators;
using Business.Models.Dtos;
using FluentAssertions;
using Xunit;

namespace API.Tests.Validators;

public class ReservationRequestValidatorTests
{
    private readonly ReservationRequestValidator _validator;

    public ReservationRequestValidatorTests()
    {
        _validator = new ReservationRequestValidator();
    }

    [Fact]
    public void StartDate_ShouldFail_WhenEmpty()
    {
        // Arrange
        var request = new ReservationRequest { StartDate = default };

        // Act
        var validationResult = _validator.Validate(request);
            
        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(ReservationRequest.StartDate) && e.ErrorMessage == "The start date of your reservation is required.");
    }

    [Fact]
    public void StartDate_ShouldFail_WhenNotWithinNext24Hours()
    {
        // Arrange
        var request = new ReservationRequest { StartDate = DateTime.UtcNow.AddHours(25) };
        
        // Act
        var validationResult = _validator.Validate(request);
            
        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(ReservationRequest.StartDate) && e.ErrorMessage == "The start date of your reservation must be within the next 24 hours.");
    }
    
    [Fact]
    public void StartDate_ShouldPass_WhenWithinNext24Hours()
    {
        // Arrange
        var request = new ReservationRequest { StartDate = DateTime.UtcNow.AddHours(2) };
        
        // Act
        var validationResult = _validator.Validate(request);
        
        //Assert
        validationResult.Errors.Should().NotContain(e => e.PropertyName == nameof(ReservationRequest.StartDate));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(121)]
    public void DurationInMinutes_ShouldFail_WhenOutOfRange(int durationInMinutes)
    {
        // Arrange
        var request = new ReservationRequest { DurationInMinutes = durationInMinutes };

        // Act
        var validationResult = _validator.Validate(request);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(ReservationRequest.DurationInMinutes) && e.ErrorMessage == "The duration of your reservation must be between 1 and 120 minutes.");
    }

    [Fact]
    public void DurationInMinutes_ShouldPass_WhenBetween1And120()
    {
        // Arrange
        var request = new ReservationRequest { DurationInMinutes = 60 };
    
        // Act
        var validationResult = _validator.Validate(request);
        
        //Assert
        validationResult.Errors.Should().NotContain(e => e.PropertyName == nameof(ReservationRequest.DurationInMinutes));
    }
    
    [Fact]
    public void Validator_ShouldPass_WhenEverythingIsValid()
    {
        // Arrange
        var request = new ReservationRequest
        {
            DurationInMinutes = 60, 
            StartDate = DateTime.UtcNow.AddHours(12)
        };
    
        // Act
        var validationResult = _validator.Validate(request);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
    }
}
