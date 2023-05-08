using Business.Models.Entities;

namespace Business.Interfaces;

public interface ICarRepository
{
    Task<Car> CreateCarAsync(Car car);
    
    Car? GetCarByUniqueIdentifier(string uniqueIdentifier);
    
    Task<Car?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<Car>> GetAllAsync();
    
    Task UpdateAsync(Car existingCar);
    
    Task DeleteAsync(Car carToDelete);
}
