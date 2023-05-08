using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Business.Interfaces;
using Business.Models;
using Business.Models.Dtos;
using Business.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class ReservationControllerTests
{
    private readonly ReservationController _sut;
    private readonly Mock<IReservationService> _reservationServiceMock;

    public ReservationControllerTests()
    {
        _reservationServiceMock = new Mock<IReservationService>();
        _sut = new ReservationController(_reservationServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnNoContent_WhenNoReservations()
    {
        // Arrange
        _reservationServiceMock
            .Setup(svc => svc.GetAllAsync())
            .ReturnsAsync(Enumerable.Empty<Reservation>());

        // Act
        var result = await _sut.GetAll();

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnReservations_WhenReservationsExist()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new Reservation { Id = Guid.NewGuid() },
            new Reservation { Id = Guid.NewGuid() }
        };

        _reservationServiceMock
            .Setup(svc => svc.GetAllAsync())
            .ReturnsAsync(reservations);

        // Act
        var result = await _sut.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(reservations);
    }

    [Fact]
    public async Task Post_ShouldReturnConflict_WhenReservationNotCreated()
    {
        // Arrange
        var newReservation = new ReservationRequest();
        var failureResult = Result<ReservationResponse>.FailureResult("Error message");

        _reservationServiceMock
            .Setup(svc => svc.CreateReservationAsync(newReservation))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _sut.Post(newReservation);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = failureResult.Message });
    }

    [Fact]
    public async Task Post_ShouldReturnCreated_WhenReservationCreated()
    {
        // Arrange
        var newReservation = new ReservationRequest();
        var reservationResponse = new ReservationResponse { ReservationId = Guid.NewGuid() };
        var successResult = Result<ReservationResponse>.SuccessResult(reservationResponse);

        _reservationServiceMock
            .Setup(svc => svc.CreateReservationAsync(newReservation))
            .ReturnsAsync(successResult);

        // Act
        var result = await _sut.Post(newReservation);

        // Assert
        result.Should().BeOfType<CreatedResult>()
            .Which.Value.Should().BeEquivalentTo(reservationResponse);
    }
}
