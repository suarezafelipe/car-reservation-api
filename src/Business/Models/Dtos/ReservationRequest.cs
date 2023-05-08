namespace Business.Models.Dtos;

public class ReservationRequest
{
    public DateTime StartDate { get; set; }

    public int DurationInMinutes { get; set; }
}
