using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models.Dtos;
using Business.Models.Entities;
using Business.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Business.Tests;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly ReservationService _reservationService;

    public ReservationServiceTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _reservationService = new ReservationService(_reservationRepositoryMock.Object);
    }

    // Test for GetAllAsync
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllReservations()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new Reservation { Id = Guid.NewGuid() },
            new Reservation { Id = Guid.NewGuid() },
            new Reservation { Id = Guid.NewGuid() }
        };

        _reservationRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(reservations);

        // Act
        var result = await _reservationService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(reservations);
        _reservationRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    // Test for CreateReservationAsync
    [Fact]
    public async Task CreateReservationAsync_ShouldCreateReservation_WhenCarsAreAvailable()
    {
        // Arrange
        var reservationRequest = new ReservationRequest
        {
            StartDate = DateTime.UtcNow,
            DurationInMinutes = 60
        };
        var availableCars = new List<Car>
        {
            new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "123456" }
        };

        _reservationRepositoryMock.Setup(repo => repo.GetAvailableCarsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(availableCars);
        _reservationRepositoryMock.Setup(repo => repo.CreateReservationAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

        // Act
        var result = await _reservationService.CreateReservationAsync(reservationRequest);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data?.Model.Should().Be(availableCars[0].Model);
        result.Data?.Make.Should().Be(availableCars[0].Make);
        result.Data?.CarUniqueIdentifier.Should().Be(availableCars[0].UniqueIdentifier);
        _reservationRepositoryMock.Verify(repo => repo.GetAvailableCarsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _reservationRepositoryMock.Verify(repo => repo.CreateReservationAsync(It.IsAny<Reservation>()), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldReturnFailureResult_WhenNoCarsAreAvailable()
    {
        // Arrange
        var reservationRequest = new ReservationRequest
        {
            StartDate = DateTime.UtcNow,
            DurationInMinutes = 60
        };
        // ReSharper disable once CollectionNeverUpdated.Local
        var availableCars = new List<Car>();

        _reservationRepositoryMock.Setup(repo => repo.GetAvailableCarsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(availableCars);

        // Act
        var result = await _reservationService.CreateReservationAsync(reservationRequest);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("No cars available for the selected dates.");
        _reservationRepositoryMock.Verify(repo => repo.GetAvailableCarsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _reservationRepositoryMock.Verify(repo => repo.CreateReservationAsync(It.IsAny<Reservation>()), Times.Never);
    }
}
