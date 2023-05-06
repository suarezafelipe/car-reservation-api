namespace Business.Entities;

public class Car
{
    public Guid Id { get; set; }
    
    public string? Make { get; set; }
    
    public string? Model { get; set; }
    
    public string? UniqueIdentifier { get; set; }
}
