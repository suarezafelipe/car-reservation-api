using Business.Interfaces;
using Business.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        
        return reservations.Any()
            ? Ok(reservations)
            : NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(ReservationRequest newReservation)
    {
        var reservationResult = await _reservationService.CreateReservationAsync(newReservation);

        return reservationResult.Success 
            ? Created($"/api/v1/reservation/{reservationResult.Data?.ReservationId}", reservationResult.Data)
            : Conflict(new { message = reservationResult.Message });
    }
}
