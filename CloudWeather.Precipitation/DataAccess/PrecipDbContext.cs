namespace CloudWeather.Precipitation.DataAccess;

using Microsoft.EntityFrameworkCore;

public class PrecipDbContext : DbContext
{
    public PrecipDbContext()
    {

    }

    public PrecipDbContext(DbContextOptions opts) : base(opts)
    {

    }


    public DbSet<Precipitation> Precipitations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentityTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Precipitation>(b => { b.ToTable("precipitation"); });
    }
}
