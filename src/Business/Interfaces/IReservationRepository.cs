using Business.Models.Entities;

namespace Business.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    
    Task CreateReservationAsync(Reservation newReservation);
    
    Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate);
}
