using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models.Entities;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data.Tests;

public class ReservationRepositoryTests
{
    private static ApplicationDbContext CreateUniqueContext()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllReservations()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var testReservation1 = new Reservation
        {
            Id = Guid.NewGuid(), CarId = Guid.NewGuid(), ReservationStart = DateTime.UtcNow.AddHours(1),
            ReservationEnd = DateTime.UtcNow.AddHours(3)
        };
        var testReservation2 = new Reservation
        {
            Id = Guid.NewGuid(), CarId = Guid.NewGuid(), ReservationStart = DateTime.UtcNow.AddHours(2),
            ReservationEnd = DateTime.UtcNow.AddHours(4)
        };
        context.Reservations.AddRange(testReservation1, testReservation2);
        await context.SaveChangesAsync();

        IReservationRepository reservationRepository = new ReservationRepository(context);

        // Act
        var result = await reservationRepository.GetAllAsync();

        var resultList = result.ToList();

        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.Count.Should().Be(2);
        resultList.Should().ContainEquivalentOf(testReservation1);
        resultList.Should().ContainEquivalentOf(testReservation2);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldAddReservationToDatabase()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var testReservation = new Reservation
        {
            Id = Guid.NewGuid(), CarId = Guid.NewGuid(), ReservationStart = DateTime.UtcNow.AddHours(1),
            ReservationEnd = DateTime.UtcNow.AddHours(3)
        };
        IReservationRepository reservationRepository = new ReservationRepository(context);

        // Act
        await reservationRepository.CreateReservationAsync(testReservation);

        // Assert
        var createdReservation = await context.Reservations.FindAsync(testReservation.Id);
        createdReservation.Should().NotBeNull();
        createdReservation.Should().BeEquivalentTo(testReservation);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldReturnAvailableCars()
    {
        // Arrange
        await using var context = CreateUniqueContext();

        // Add test data
        var car1 = new Car {Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};
        var car2 = new Car {Id = Guid.NewGuid(), Make = "Honda", Model = "Civic", UniqueIdentifier = "C789012"};
        context.Cars.AddRange(car1, car2);

        var reservation1 = new Reservation
        {
            Id = Guid.NewGuid(), CarId = car1.Id, ReservationStart = DateTime.UtcNow.AddHours(1),
            ReservationEnd = DateTime.UtcNow.AddHours(3)
        };
        context.Reservations.Add(reservation1);

        await context.SaveChangesAsync();

        IReservationRepository reservationRepository = new ReservationRepository(context);

        // Act
        var startDate = DateTime.UtcNow.AddHours(1);
        var endDate = DateTime.UtcNow.AddHours(3);
        var result = await reservationRepository.GetAvailableCarsAsync(startDate, endDate);

        var resultList = result.ToList();

        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.Count.Should().Be(1);
        resultList.Should().ContainEquivalentOf(car2);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldReturnEmpty_WhenNoCarsAreAvailable()
    {
        // Arrange
        await using var context = CreateUniqueContext();

        // Add test data
        var car1 = new Car {Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456"};
        var car2 = new Car {Id = Guid.NewGuid(), Make = "Honda", Model = "Civic", UniqueIdentifier = "C789012"};
        context.Cars.AddRange(car1, car2);

        var reservation1 = new Reservation
        {
            Id = Guid.NewGuid(), CarId = car1.Id, ReservationStart = DateTime.UtcNow.AddHours(1),
            ReservationEnd = DateTime.UtcNow.AddHours(3)
        };
        var reservation2 = new Reservation
        {
            Id = Guid.NewGuid(), CarId = car2.Id, ReservationStart = DateTime.UtcNow.AddHours(2),
            ReservationEnd = DateTime.UtcNow.AddHours(4)
        };
        context.Reservations.AddRange(reservation1, reservation2);

        await context.SaveChangesAsync();

        IReservationRepository reservationRepository = new ReservationRepository(context);

        // Act
        var startDate = DateTime.UtcNow.AddHours(1);
        var endDate = DateTime.UtcNow.AddHours(3);
        var result = await reservationRepository.GetAvailableCarsAsync(startDate, endDate);

        var resultList = result.ToList();

        // Assert
        resultList.Should().BeEmpty();
    }
}
