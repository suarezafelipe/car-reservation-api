using Business.Entities;
using Business.Interfaces;

namespace Data.Repositories;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Car> CreateCarAsync(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public Car? GetCarByUniqueIdentifier(string uniqueIdentifier) 
        => _context.Cars.FirstOrDefault(c => c.UniqueIdentifier == uniqueIdentifier);
}
