namespace Business.Models.Dtos;

public class ReservationResponse
{
    public Guid ReservationId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int DurationInMinutes { get; set; }
    
    public string? Make { get; set; }
    
    public string? Model { get; set; }

    public string? CarUniqueIdentifier { get; set; }
}
