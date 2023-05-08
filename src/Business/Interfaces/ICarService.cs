using Business.Models.Entities;

namespace Business.Interfaces;

public interface ICarService
{
    Task<Car> CreateCarAsync(Car car);
    
    bool IsUniqueIdentifierUnique(string uniqueIdentifier);
    
    Task<Car?> GetByIdAsync(Guid parsedId);
    
    Task<IEnumerable<Car>> GetAllAsync();
    
    Task UpdateAsync(Car carToUpdate);
    
    Task DeleteAsync(Guid id);
}
