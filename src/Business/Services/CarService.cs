using Business.Interfaces;
using Business.Models;
using Business.Models.Entities;

namespace Business.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }
    
    public async Task<Car> CreateCarAsync(Car car)
    {
        car.Id = Guid.NewGuid();
        return await _carRepository.CreateCarAsync(car);
    }

    public bool IsUniqueIdentifierUnique(string uniqueIdentifier)
    {
        var carWithSameIdentifier = _carRepository.GetCarByUniqueIdentifier(uniqueIdentifier);
        return carWithSameIdentifier is null;
    }

    public async Task<Car?> GetByIdAsync(Guid id) => await _carRepository.GetByIdAsync(id);
    
    public async Task<IEnumerable<Car>> GetAllAsync() => await _carRepository.GetAllAsync();
    
    public async Task<Result<bool>> UpdateAsync(Car carToUpdate)
    {
        var existingCar = await GetByIdAsync(carToUpdate.Id);
    
        if (existingCar == null)
        {
            return Result<bool>.FailureResult($"Car with id: {carToUpdate.Id} was not found!");
        }

        UpdateCarProperties(existingCar, carToUpdate);
    
        await _carRepository.UpdateAsync(existingCar);
        return Result<bool>.SuccessResult(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var carToDelete = await GetByIdAsync(id);
    
        if (carToDelete == null)
        {
            return Result<bool>.FailureResult($"Car with id: {id} was not found!");
        }
    
        await _carRepository.DeleteAsync(carToDelete);
        return Result<bool>.SuccessResult(true);
    }
    
    private static void UpdateCarProperties(Car existingCar, Car updatedCar)
    {
        if (!string.IsNullOrEmpty(updatedCar.Make))
        {
            existingCar.Make = updatedCar.Make;
        }

        if (!string.IsNullOrEmpty(updatedCar.Model))
        {
            existingCar.Model = updatedCar.Model;
        }
    
        if (!string.IsNullOrEmpty(updatedCar.UniqueIdentifier))
        {
            existingCar.UniqueIdentifier = updatedCar.UniqueIdentifier;
        }
    }
}
