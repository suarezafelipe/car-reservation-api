using Business.Entities;

namespace Business.Interfaces;

public interface ICarRepository
{
    Task<Car> CreateCarAsync(Car car);
    
    Car? GetCarByUniqueIdentifier(string uniqueIdentifier);
}
