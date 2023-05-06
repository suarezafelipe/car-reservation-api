using Business.Entities;

namespace Business.Interfaces;

public interface ICarService
{
    Task<Car> CreateCar(Car car);
}
