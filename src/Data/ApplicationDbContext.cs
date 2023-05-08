using Business.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; } = null!;
    
    public DbSet<Reservation> Reservations { get; set; } = null!;
}
