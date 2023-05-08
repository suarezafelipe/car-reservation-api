using Business.Exceptions;
using Business.Interfaces;
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
    
    public async Task UpdateAsync(Car carToUpdate)
    {
        var existingCar = await GetByIdAsync(carToUpdate.Id);
        
        if (existingCar == null)
        {
            throw new CarNotFoundException($"Car with id: {carToUpdate.Id} was not found!");
        }

        if (!string.IsNullOrEmpty(carToUpdate.Make))
        {
            existingCar.Make = carToUpdate.Make;
        }

        if (!string.IsNullOrEmpty(carToUpdate.Model))
        {
            existingCar.Model = carToUpdate.Model;
        }
        
        if (!string.IsNullOrEmpty(carToUpdate.UniqueIdentifier))
        {
            existingCar.UniqueIdentifier = carToUpdate.UniqueIdentifier;
        }
        
        await _carRepository.UpdateAsync(existingCar);
    }

    public async Task DeleteAsync(Guid id)
    {
        var carToDelete = await GetByIdAsync(id);
        
        if (carToDelete == null)
        {
            throw new CarNotFoundException($"Car with id: {id} was not found!");
        }
        
        await _carRepository.DeleteAsync(carToDelete);
    }
}
