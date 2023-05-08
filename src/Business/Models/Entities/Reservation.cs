namespace Business.Models.Entities;

public class Reservation
{
    public Guid Id { get; set; }

    public Guid CarId { get; set; }

    public DateTime ReservationStart { get; set; }

    public int DurationInMinutes { get; set; }

    public DateTime ReservationEnd { get; set; }
}
