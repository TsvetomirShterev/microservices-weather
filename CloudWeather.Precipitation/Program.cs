using CloudWeather.Precipitation.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrecipDbContext>
(
    opts =>
    {
        opts.EnableSensitiveDataLogging();
        opts.EnableDetailedErrors();
        opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, PrecipDbContext db) =>
{
    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);

    if (days == null || days < 1 || days > 30)
    {
        return Results.BadRequest("Please provide a 'days' query between 1 and 30");
    }

    var results = await db
    .Precipitations
    .Where(p => p.ZipCode == zip && p.CreatedOn > startDate)
    .ToListAsync();

    return Results.Ok(results);
});

app.Run();
