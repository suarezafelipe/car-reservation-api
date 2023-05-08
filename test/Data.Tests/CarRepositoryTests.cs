using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Models.Entities;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data.Tests;

public class CarRepositoryTests
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
    public async Task GetByIdAsync_ShouldReturnCar_WhenCarExists()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var testCar = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456" };
        context.Cars.Add(testCar);
        await context.SaveChangesAsync();

        var carRepository = new CarRepository(context);

        // Act
        var result = await carRepository.GetByIdAsync(testCar.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testCar);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCarDoesNotExist()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var carRepository = new CarRepository(context);

        // Act
        var result = await carRepository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCars()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var testCar1 = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "C123456" };
        var testCar2 = new Car { Id = Guid.NewGuid(), Make = "Honda", Model = "Civic", UniqueIdentifier = "C789012" };
        context.Cars.Add(testCar1);
        context.Cars.Add(testCar2);
        await context.SaveChangesAsync();

        var carRepository = new CarRepository(context);

        // Act
        var result = await carRepository.GetAllAsync();

        var resultList = result.ToList();
        
        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.Count.Should().Be(2);
        resultList.Should().ContainEquivalentOf(testCar1);
        resultList.Should().ContainEquivalentOf(testCar2);
    }
    
    [Fact]
    public async Task CreateCarAsync_ShouldAddCarToDatabase()
    {
        // Arrange
        await using var context = CreateUniqueContext();
        var testCar = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "123456" };
        var carRepository = new CarRepository(context);

        // Act
        var createdCar = await carRepository.CreateCarAsync(testCar);

        // Assert
        createdCar.Should().NotBeNull();
        createdCar.Should().BeEquivalentTo(testCar);
        context.Cars.Should().ContainEquivalentOf(testCar);
    }
    
     [Fact]
        public async Task UpdateAsync_ShouldUpdateCarInDatabase()
        {
            // Arrange
            await using var context = CreateUniqueContext();
            var testCar = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "123456" };
            context.Cars.Add(testCar);
            await context.SaveChangesAsync();

            var carRepository = new CarRepository(context);

            // Act
            testCar.Make = "UpdatedMake";
            testCar.Model = "UpdatedModel";
            testCar.UniqueIdentifier = "UpdatedUniqueIdentifier";
            await carRepository.UpdateAsync(testCar);

            // Assert
            var updatedCar = await context.Cars.FindAsync(testCar.Id);
            updatedCar.Should().NotBeNull();
            updatedCar.Should().BeEquivalentTo(testCar);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCarFromDatabase()
        {
            // Arrange
            await using var context = CreateUniqueContext();
            var testCar = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "123456" };
            context.Cars.Add(testCar);
            await context.SaveChangesAsync();

            var carRepository = new CarRepository(context);

            // Act
            await carRepository.DeleteAsync(testCar);

            // Assert
            var deletedCar = await context.Cars.FindAsync(testCar.Id);
            deletedCar.Should().BeNull();
        }

        [Fact]
        public void GetCarByUniqueIdentifier_ShouldReturnCarWithGivenUniqueIdentifier()
        {
            // Arrange
            using var context = CreateUniqueContext();
            var testCar = new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Camry", UniqueIdentifier = "123456" };
            context.Cars.Add(testCar);
            context.SaveChanges();

            var carRepository = new CarRepository(context);

            // Act
            var foundCar = carRepository.GetCarByUniqueIdentifier("123456");

            // Assert
            foundCar.Should().NotBeNull();
            foundCar.Should().BeEquivalentTo(testCar);
        }
}
