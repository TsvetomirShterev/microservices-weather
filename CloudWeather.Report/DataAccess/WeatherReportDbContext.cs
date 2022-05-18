namespace CloudWeather.Report.DataAccess;

using Microsoft.EntityFrameworkCore;

public class WeatherReportDbContext : DbContext
{
    public WeatherReportDbContext()
    {

    }

    public WeatherReportDbContext(DbContextOptions opts) : base(opts)
    {

    }

    public DbSet<WeatherReport> WeatherReports { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    private static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherReport>(w =>
        {
            w.ToTable("weather_report");
        });
    }
}
