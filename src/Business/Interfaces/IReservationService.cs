using Business.Models;
using Business.Models.Dtos;
using Business.Models.Entities;

namespace Business.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    
    Task<Result<ReservationResponse>> CreateReservationAsync(ReservationRequest reservation);
}
