namespace Business.Models.Entities;

/// <summary>
/// Represents a car available for reservation.
/// </summary>
public class Car
{
    /// <summary>
    /// The database unique identifier of the car.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The make of the car.
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// The model of the car.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// An unique identifier of the car, following the pattern "C-number-".
    /// </summary>
    public string? UniqueIdentifier { get; set; }
}
