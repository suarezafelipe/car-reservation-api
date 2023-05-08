using Business.Interfaces;
using Business.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
    
    [HttpGet("{id}")]
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
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var carList = await _carService.GetAllAsync();

        return carList.Any()
            ? Ok(carList)
            : NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Car newCar)
    {
        var createdCar = await _carService.CreateCarAsync(newCar);
        return Created($"/api/v1/car/{newCar.Id}", createdCar);
    }
    
    [HttpPut]
    public async Task<IActionResult> Put(Car carToUpdate)
    {
        var updateResult = await _carService.UpdateAsync(carToUpdate);
        
        return updateResult.Success
            ? Ok(carToUpdate)
            : Conflict(new { message = updateResult.Message });
    }
    
    [HttpDelete("{id}")]
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
