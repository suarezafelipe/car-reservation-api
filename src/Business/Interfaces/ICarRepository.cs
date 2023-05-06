using Business.Entities;

namespace Business.Interfaces;

public interface ICarRepository
{
    Task<Car> CreateCar(Car car);
}
