﻿namespace CloudWeather.Temperature.DataAccess;

using Microsoft.EntityFrameworkCore;

public class TemperatureDbContext : DbContext
{
    public TemperatureDbContext()
    {

    }

    public TemperatureDbContext(DbContextOptions opts) : base(opts)
    {

    }


    public DbSet<Temperature> Temperatures { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentityTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Temperature>(b => { b.ToTable("temperature"); });
    }
}
