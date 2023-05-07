using Business.Entities;
using Business.Interfaces;

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
}
