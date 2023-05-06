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
    
    public async Task<Car> CreateCar(Car car)
    {
        return await _carRepository.CreateCar(car);
    }
}
