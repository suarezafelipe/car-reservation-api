using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Business.Interfaces;
using Business.Models;
using Business.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class CarControllerTests
{
    private readonly CarController _sut;
    private readonly Mock<ICarService> _carServiceMock;

    public CarControllerTests()
    {
        _carServiceMock = new Mock<ICarService>();
        _sut = new CarController(_carServiceMock.Object);
    }

    [Fact]
    public async Task Get_ById_ShouldReturnBadRequest_WhenInvalidId()
    {
        // Act
        var result = await _sut.Get("invalid-id");

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Get_ById_ShouldReturnNoContent_WhenCarNotFound()
    {
        // Arrange
        _carServiceMock.Setup(svc => svc.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Car) null!);

        // Act
        var result = await _sut.Get(Guid.NewGuid().ToString());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Get_ById_ShouldReturnCar_WhenCarFound()
    {
        // Arrange
        var car = new Car {Id = Guid.NewGuid()};
        _carServiceMock.Setup(svc => svc.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);

        // Act
        var result = await _sut.Get(car.Id.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(car);
    }

    [Fact]
    public async Task Get_ShouldReturnNoContent_WhenNoCars()
    {
        // Arrange
        _carServiceMock
            .Setup(svc => svc.GetAllAsync())
            .ReturnsAsync(Enumerable.Empty<Car>());

        // Act
        var result = await _sut.Get();

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Get_ShouldReturnCars_WhenCarsExist()
    {
        // Arrange
        var cars = new List<Car>
        {
            new Car {Id = Guid.NewGuid()},
            new Car {Id = Guid.NewGuid()}
        };

        _carServiceMock
            .Setup(svc => svc.GetAllAsync())
            .ReturnsAsync(cars);

        // Act
        var result = await _sut.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(cars);
    }

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenCarCreated()
    {
        // Arrange
        var newCar = new Car();
        var createdCar = new Car {Id = Guid.NewGuid()};

        _carServiceMock
            .Setup(svc => svc.CreateCarAsync(newCar))
            .ReturnsAsync(createdCar);

        // Act
        var result = await _sut.Post(newCar);

        // Assert
        result.Should().BeOfType<CreatedResult>()
            .Which.Value.Should().BeEquivalentTo(createdCar);
    }

    [Fact]
    public async Task Put_ShouldReturnConflict_WhenUpdateFails()
    {
        // Arrange
        var carToUpdate = new Car {Id = Guid.NewGuid()};
        var updateResult = Result<bool>.FailureResult("Update failed");

        _carServiceMock
            .Setup(svc => svc.UpdateAsync(carToUpdate))
            .ReturnsAsync(updateResult);

        // Act
        var result = await _sut.Put(carToUpdate);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new {message = updateResult.Message});
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var carToUpdate = new Car {Id = Guid.NewGuid()};
        var updateResult = Result<bool>.SuccessResult(true);

        _carServiceMock
            .Setup(svc => svc.UpdateAsync(carToUpdate))
            .ReturnsAsync(updateResult);

        // Act
        var result = await _sut.Put(carToUpdate);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(carToUpdate);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenInvalidId()
    {
        // Act
        var result = await _sut.Delete("invalid-id");

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnConflict_WhenDeleteFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var deleteResult = Result<bool>.FailureResult("Delete failed");

        _carServiceMock
            .Setup(svc => svc.DeleteAsync(id))
            .ReturnsAsync(deleteResult);

        // Act
        var result = await _sut.Delete(id.ToString());

        // Assert
        result.Should().BeOfType<ConflictObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new {message = deleteResult.Message});
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenDeleteSucceeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var deleteResult = Result<bool>.SuccessResult(true);

        _carServiceMock
            .Setup(svc => svc.DeleteAsync(id))
            .ReturnsAsync(deleteResult);

        // Act
        var result = await _sut.Delete(id.ToString());

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
