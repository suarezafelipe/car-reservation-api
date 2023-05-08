using Business.Interfaces;
using Business.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages car operations.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    /// <summary>
    /// Retrieves a car by its ID.
    /// </summary>
    /// <param name="id">The ID of the car.</param>
    /// <returns>A car with the specified ID, or 204 if the car is not found.</returns>
    /// <response code="200">Returns the requested car.</response>
    /// <response code="204">If the car is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(string id)
    {
        if (!Guid.TryParse(id, out Guid parsedId))
        {
            return BadRequest("The input is not a valid Unique Identifier");
        }

        var car = await _carService.GetByIdAsync(parsedId);

        return car == null
            ? NoContent()
            : Ok(car);
    }

    /// <summary>
    /// Retrieves all cars.
    /// </summary>
    /// <returns>A list of all cars, or 204 if no cars are found.</returns>
    /// <response code="200">Returns the list of cars.</response>
    /// <response code="204">If no cars are found.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get()
    {
        var carList = await _carService.GetAllAsync();

        return carList.Any()
            ? Ok(carList)
            : NoContent();
    }

    /// <summary>
    /// Adds a new car.
    /// </summary>
    /// <param name="newCar">The car to be added.</param>
    /// <returns>201 Response with the created car and its ID.</returns>
    /// <response code="201">Returns the newly created car.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(Car newCar)
    {
        var createdCar = await _carService.CreateCarAsync(newCar);
        return Created($"/api/v1/car/{newCar.Id}", createdCar);
    }

    /// <summary>
    /// Updates an existing car.
    /// </summary>
    /// <param name="carToUpdate">The car to be updated.</param>
    /// <returns>The updated car, or a 409 response if an error occurs.</returns>
    /// <response code="200">Returns the updated car.</response>
    /// <response code="409">If the car could not be updated.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut]
    public async Task<IActionResult> Put(Car carToUpdate)
    {
        var updateResult = await _carService.UpdateAsync(carToUpdate);

        return updateResult.Success
            ? Ok(carToUpdate)
            : Conflict(new { message = updateResult.Message });
    }

    /// <summary>
    /// Deletes a car by its ID.
    /// </summary>
    /// <param name="id">The ID of the car to be deleted.</param>
    /// <returns>An Ok 200 response if the car is deleted, or a 409 response if an error occurs.</returns>
    /// <response code="200">Returns an Ok 200 response if the car is deleted.</response>
    /// <response code="400">If the input ID is not a valid Unique Identifier.</response>
    /// <response code="409">If the car could not be deleted.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(string id)
    {
        if (!Guid.TryParse(id, out Guid parsedId))
        {
            return BadRequest(new { message = "The input is not a valid Unique Identifier" });
        }

        var deleteResult = await _carService.DeleteAsync(parsedId);

        return deleteResult.Success
            ? Ok()
            : Conflict(new { message = deleteResult.Message });
    }
}
