using Business.Interfaces;
using Business.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task CreateReservationAsync(Reservation newReservation)
    {
        _context.Add(newReservation);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
    {
        var availableCars = await (from car in _context.Cars
            join reservation in _context.Reservations
                on car.Id equals reservation.CarId into carReservations
            from carReservation in carReservations.DefaultIfEmpty()
            where carReservation == null
                  || (
                      (carReservation.ReservationEnd <= startDate || carReservation.ReservationStart >= endDate)
                      && !_context.Reservations.Any(r =>
                          r.CarId == car.Id
                          && r.ReservationStart < endDate
                          && r.ReservationEnd > startDate))
            select new Car
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                UniqueIdentifier = car.UniqueIdentifier
            }).Distinct().ToListAsync();

        return availableCars;
    }
}
