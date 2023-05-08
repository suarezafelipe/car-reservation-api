using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models.Entities;
using Business.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Business.Tests;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _carService = new CarService(_carRepositoryMock.Object);
    }


    [Fact]
    public async Task CreateCarAsync_ShouldCreateCar()
    {
        // Arrange
        var car = new Car {Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};

        _carRepositoryMock.Setup(repo => repo.CreateCarAsync(It.IsAny<Car>()))
            .ReturnsAsync((Car c) => c);

        // Act
        var createdCar = await _carService.CreateCarAsync(car);

        // Assert
        createdCar.Should().NotBeNull();
        createdCar.Id.Should().NotBeEmpty();
        createdCar.Make.Should().Be(car.Make);
        createdCar.Model.Should().Be(car.Model);
        createdCar.UniqueIdentifier.Should().Be(car.UniqueIdentifier);
        _carRepositoryMock.Verify(repo => repo.CreateCarAsync(It.IsAny<Car>()), Times.Once);
    }


    [Fact]
    public void IsUniqueIdentifierUnique_ShouldReturnTrue_WhenUniqueIdentifierIsUnique()
    {
        // Arrange
        _carRepositoryMock.Setup(repo => repo.GetCarByUniqueIdentifier(It.IsAny<string>()))
            .Returns((Car) null!);

        // Act
        var isUnique = _carService.IsUniqueIdentifierUnique("C123456");

        // Assert
        isUnique.Should().BeTrue();
    }

    [Fact]
    public void IsUniqueIdentifierUnique_ShouldReturnFalse_WhenUniqueIdentifierIsNotUnique()
    {
        // Arrange
        var car = new Car {Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};
        _carRepositoryMock.Setup(repo => repo.GetCarByUniqueIdentifier(It.IsAny<string>()))
            .Returns(car);

        // Act
        var isUnique = _carService.IsUniqueIdentifierUnique("C123456");

        // Assert
        isUnique.Should().BeFalse();
    }

    // Test for GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCar_WhenCarExists()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var car = new Car {Id = carId, Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync(car);

        // Act
        var result = await _carService.GetByIdAsync(carId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(car);
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCarDoesNotExist()
    {
        // Arrange
        var carId = Guid.NewGuid();

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync((Car) null!);

        // Act
        var result = await _carService.GetByIdAsync(carId);

        // Assert
        result.Should().BeNull();
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
    }

// Test for GetAllAsync
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCars()
    {
        // Arrange
        var car1 = new Car {Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};
        var car2 = new Car {Id = Guid.NewGuid(), Make = "Honda", Model = "Civic", UniqueIdentifier = "C789012"};
        var cars = new List<Car> {car1, car2};

        _carRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cars);

        // Act
        var result = await _carService.GetAllAsync();

        var resultList = result.ToList();

        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.Count().Should().Be(2);
        resultList.Should().ContainEquivalentOf(car1);
        resultList.Should().ContainEquivalentOf(car2);
        _carRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

// Test for UpdateAsync
    [Fact]
    public async Task UpdateAsync_ShouldUpdateCar_WhenCarExists()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var existingCar = new Car {Id = carId, Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};
        var updatedCar = new Car
            {Id = carId, Make = "UpdatedToyota", Model = "UpdatedCamry", UniqueIdentifier = "C654321"};

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync(existingCar);
        _carRepositoryMock.Setup(repo => repo.UpdateAsync(existingCar)).Returns(Task.CompletedTask);

        // Act
        var result = await _carService.UpdateAsync(updatedCar);

        // Assert
        result.Success.Should().BeTrue();
        existingCar.Should().BeEquivalentTo(updatedCar, options => options.Excluding(car => car.Id));
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
        _carRepositoryMock.Verify(repo => repo.UpdateAsync(existingCar), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailureResult_WhenCarDoesNotExist()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var updatedCar = new Car
            {Id = carId, Make = "UpdatedToyota", Model = "UpdatedCamry", UniqueIdentifier = "C654321"};

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync((Car) null!);

        // Act
        var result = await _carService.UpdateAsync(updatedCar);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"Car with id: {carId} was not found!");
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
        _carRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Car>()), Times.Never);
    }

// Test for DeleteAsync
    [Fact]
    public async Task DeleteAsync_ShouldDeleteCar_WhenCarExists()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var carToDelete = new Car {Id = carId, Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync(carToDelete);
        _carRepositoryMock.Setup(repo => repo.DeleteAsync(carToDelete)).Returns(Task.CompletedTask);

        // Act
        var result = await _carService.DeleteAsync(carId);

        // Assert
        result.Success.Should().BeTrue();
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
        _carRepositoryMock.Verify(repo => repo.DeleteAsync(carToDelete), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureResult_WhenCarDoesNotExist()
    {
        // Arrange
        var carId = Guid.NewGuid();

        _carRepositoryMock.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync((Car) null!);

        // Act
        var result = await _carService.DeleteAsync(carId);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"Car with id: {carId} was not found!");
        _carRepositoryMock.Verify(repo => repo.GetByIdAsync(carId), Times.Once);
        _carRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Car>()), Times.Never);
    }
}
