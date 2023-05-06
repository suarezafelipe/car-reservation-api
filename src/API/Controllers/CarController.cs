using Business.Entities;
using Business.Interfaces;
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

    [HttpPost]
    public async Task<IActionResult> Post(Car newCar)
    {
        Car createdCar = await _carService.CreateCar(newCar);
        return Created($"/api/v1/car/{newCar.Id}", createdCar);
    }
}
