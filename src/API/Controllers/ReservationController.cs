using Business.Interfaces;
using Business.Models.Dtos;
using Business.Models.Entities;
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
        var reservationDetails = await _reservationService.CreateReservationAsync(newReservation);
        return Created($"/api/v1/reservation/{reservationDetails.ReservationId}", reservationDetails);
    }
}
