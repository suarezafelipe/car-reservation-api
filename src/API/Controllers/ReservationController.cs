using Business.Interfaces;
using Business.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
        
    /// <summary>
    /// Gets all reservations.
    /// </summary>
    /// <remarks>
    /// Returns a list of all reservations.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        
        return reservations.Any()
            ? Ok(reservations)
            : NoContent();
    }
        
    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <remarks>
    /// Creates a new reservation based on the given reservation details.
    /// </remarks>
    /// <param name="newReservation">The reservation details.</param>
    /// <response code="201">Returns the newly created reservation.</response>
    /// <response code="409">If the reservation could not be created due to a conflict with an existing resource.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post(ReservationRequest newReservation)
    {
        var reservationResult = await _reservationService.CreateReservationAsync(newReservation);

        return reservationResult.Success 
            ? Created($"/api/v1/reservation/{reservationResult.Data?.ReservationId}", reservationResult.Data)
            : Conflict(new { message = reservationResult.Message });
    }
}
