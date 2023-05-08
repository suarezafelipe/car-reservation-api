using Business.Models.Dtos;
using Business.Models.Entities;

namespace Business.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    
    Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservation);
}
