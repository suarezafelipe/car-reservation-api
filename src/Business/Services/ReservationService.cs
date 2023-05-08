using Business.Interfaces;
using Business.Models;
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
    

    public async Task<OperationResult<ReservationResponse>> CreateReservationAsync(ReservationRequest reservationRequest)
    {
        var reservation = CreateReservationFromRequest(reservationRequest);
        var availableCars = await GetAvailableCarsAsync(reservation.ReservationStart, reservation.ReservationEnd);
        var reservedCar = availableCars.FirstOrDefault();

        if (reservedCar == null)
        {
            return OperationResult<ReservationResponse>.FailureResult("No cars available for the selected dates.");
        }

        await SaveReservationAsync(reservation, reservedCar);
        var reservationResponse = CreateReservationResponse(reservation, reservedCar);

        return OperationResult<ReservationResponse>.SuccessResult(reservationResponse);
    }


    private Reservation CreateReservationFromRequest(ReservationRequest reservationRequest)
    {
        return new Reservation
        {
            ReservationStart = reservationRequest.StartDate,
            DurationInMinutes = reservationRequest.DurationInMinutes,
            ReservationEnd = reservationRequest.StartDate.AddMinutes(reservationRequest.DurationInMinutes)
        };
    }

    private async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime reservationStart, DateTime reservationEnd)
    {
        var availableCars = await _reservationRepository.GetAvailableCarsAsync(reservationStart, reservationEnd);
        return availableCars.ToList();
    }

    private async Task SaveReservationAsync(Reservation reservation, Car reservedCar)
    {
        reservation.CarId = reservedCar.Id;
        reservation.Id = Guid.NewGuid();
        await _reservationRepository.CreateReservationAsync(reservation);
    }

    private ReservationResponse CreateReservationResponse(Reservation reservation, Car reservedCar)
    {
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
