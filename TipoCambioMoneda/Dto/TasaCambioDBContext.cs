using Microsoft.EntityFrameworkCore;

public class TasaCambioDBContext : DbContext
{
    public TasaCambioDBContext(DbContextOptions<TasaCambioDBContext> options) : base(options)
    {
        
    }
    public DbSet<BitacoraTasaCambio> BitacoraTasaCambios {get; set;}
}