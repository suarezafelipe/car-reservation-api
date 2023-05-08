using Business.Models;
using Business.Models.Entities;

namespace Business.Interfaces;

public interface ICarService
{
    Task<Car> CreateCarAsync(Car car);
    
    bool IsUniqueIdentifierUnique(string uniqueIdentifier);
    
    Task<Car?> GetByIdAsync(Guid parsedId);
    
    Task<IEnumerable<Car>> GetAllAsync();

    Task<Result<bool>> UpdateAsync(Car carToUpdate);

    Task<Result<bool>> DeleteAsync(Guid id);
}
