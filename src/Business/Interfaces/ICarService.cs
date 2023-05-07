using Business.Entities;

namespace Business.Interfaces;

public interface ICarService
{
    Task<Car> CreateCarAsync(Car car);
    
    bool IsUniqueIdentifierUnique(string uniqueIdentifier);
}
