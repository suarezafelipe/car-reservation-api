using API.Validators;
using Business.Interfaces;
using Business.Models.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace API.Tests.Validators;

public class CarValidatorTests
{
    private readonly CarValidator _validator;
    private readonly Mock<ICarService> _carServiceMock;

    public CarValidatorTests()
    {
        _carServiceMock = new Mock<ICarService>();
        _validator = new CarValidator(_carServiceMock.Object);
    }

    [Fact]
    public void Make_ShouldFail_WhenEmpty()
    {
        // Arrange
        var car = new Car { Make = string.Empty };

        // Act
        var validationResult = _validator.Validate(car);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(Car.Make) && e.ErrorMessage == "Make is required.");
    }

    [Fact]
    public void Model_ShouldFail_WhenEmpty()
    {
        // Arrange
        var car = new Car { Model = string.Empty };

        // Act
        var validationResult = _validator.Validate(car);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(Car.Model) && e.ErrorMessage == "Model is required.");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("Cabc")]
    public void UniqueIdentifier_ShouldFail_WhenInvalidFormat(string uniqueIdentifier)
    {
        // Arrange
        var car = new Car { UniqueIdentifier = uniqueIdentifier };

        // Act
        var validationResult = _validator.Validate(car);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(Car.UniqueIdentifier) && e.ErrorMessage == "UniqueIdentifier should start with 'C' followed by numbers only.");
    }

    [Fact]
    public void UniqueIdentifier_ShouldFail_WhenNotUnique()
    {
        // Arrange
        var car = new Car { UniqueIdentifier = "C123" };
        _carServiceMock.Setup(service => service.IsUniqueIdentifierUnique(It.IsAny<string>())).Returns(false);

        // Act
        var validationResult = _validator.Validate(car);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == nameof(Car.UniqueIdentifier) && e.ErrorMessage == "UniqueIdentifier must be unique.");
    }

    [Fact]
    public void Validator_ShouldPass_WhenEverythingIsValid()
    {
        // Arrange
        var car = new Car
        {
            Make = "Toyota",
            Model = "Camry",
            UniqueIdentifier = "C123"
        };
        _carServiceMock.Setup(service => service.IsUniqueIdentifierUnique(It.IsAny<string>())).Returns(true);

        // Act
        var validationResult = _validator.Validate(car);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }
}
