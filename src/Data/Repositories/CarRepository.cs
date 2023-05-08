using Business.Interfaces;
using Business.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Car?> GetByIdAsync(Guid id) => await _context.Cars.FindAsync(id);

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        return await _context.Cars
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Car> CreateCarAsync(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        return car;
    }
    
    public async Task UpdateAsync(Car existingCar)
    {
        _context.Update(existingCar);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Car carToDelete)
    {
        _context.Remove(carToDelete);
        await _context.SaveChangesAsync();
    }

    public Car? GetCarByUniqueIdentifier(string uniqueIdentifier) 
        => _context.Cars.FirstOrDefault(c => c.UniqueIdentifier == uniqueIdentifier);
}
