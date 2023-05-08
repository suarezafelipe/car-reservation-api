using Business.Interfaces;
using Business.Models.Dtos;
using Business.Models.Entities;

namespace Business.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync() => await _reservationRepository.GetAllAsync();
    

    public async Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservationRequest)
    {
        var reservation = new Reservation
        {
            ReservationStart = reservationRequest.StartDate,
            DurationInMinutes = reservationRequest.DurationInMinutes,
            ReservationEnd = reservationRequest.StartDate.AddMinutes(reservationRequest.DurationInMinutes)
        };

        var availableCars = await _reservationRepository.GetAvailableCarsAsync(reservation.ReservationStart, reservation.ReservationEnd);

        var availableCarsList = availableCars.ToList();
        
        if (!availableCarsList.Any())
        {
            throw new Exception("No cars available for the selected dates.");
        }
        
        // Take the first available car. At this point we've already validated that there is at least one available car.
        var reservedCar = availableCarsList.First();
        
        // Add that car to the reservation. and store a new the reservation in the DB
        reservation.CarId = reservedCar.Id;
        reservation.Id = Guid.NewGuid();
        await _reservationRepository.CreateReservationAsync(reservation);

        return new ReservationResponse
        {
            ReservationId = reservation.Id,
            StartTime = reservation.ReservationStart,
            EndTime = reservation.ReservationEnd,
            DurationInMinutes = reservation.DurationInMinutes,
            Model = reservedCar.Model,
            Make = reservedCar.Make,
            CarUniqueIdentifier = reservedCar.UniqueIdentifier
        };
    }
}
