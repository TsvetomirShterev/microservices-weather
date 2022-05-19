using CloudWeather.DataLoader.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var servicesConfig = config.GetSection("Services");

var tempServiceConfig = servicesConfig.GetSection("Temperature");
var tempServiceHost = tempServiceConfig.GetSection("Host");
var tempServicePort = tempServiceConfig.GetSection("Port");

var precipServiceConfig = servicesConfig.GetSection("Precipitation");
var precipServiceHost = precipServiceConfig.GetSection("Host");
var precipServicPort = precipServiceConfig.GetSection("Port");

var zipCodes = new List<string>
{
    "73026",
    "68104",
    "04401",
    "32808",
    "19717"
};

Console.WriteLine("Starting Data Load");

var temperatureHttpClient = new HttpClient();
temperatureHttpClient.BaseAddress = new Uri($"http://{tempServiceHost.Value}:{tempServicePort.Value}");

var precipitationhttpClient = new HttpClient();
precipitationhttpClient.BaseAddress = new Uri($"http://{precipServiceHost.Value}:{precipServicPort.Value}");

foreach (var zip in zipCodes)
{
    Console.WriteLine($"Processing Zip Code {zip}");
    var from = DateTime.Now.AddDays(-2);
    var thru = DateTime.Now;

    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
    {
        var temps = PostTemp(zip, day, temperatureHttpClient);
        PostPrecip(temps[0], zip, day, precipitationhttpClient);
    }
}

void PostPrecip(int lowTemp, string zip, DateTime day, HttpClient precipitationhttpClient)
{
    var rand = new Random();
    var isPrecip = rand.Next(2) < 1;

    PrecipitationModel precipitation;

    if (isPrecip)
    {
        var precipInches = rand.Next(1, 16);
        if (lowTemp < 32)
        {
            precipitation = new PrecipitationModel
            {
                AmountInches = precipInches,
                WeatherType = "snow",
                ZipCode = zip,
                CreatedOn = day
            };
        }
        else
        {
            precipitation = new PrecipitationModel
            {
                AmountInches = precipInches,
                WeatherType = "rain",
                ZipCode = zip,
                CreatedOn = day
            };
        }
    }
    else
    {
        precipitation = new PrecipitationModel
        {
            AmountInches = 0,
            WeatherType = "none",
            ZipCode = zip,
            CreatedOn = day
        };
    }

    var precipResponse = precipitationhttpClient
        .PostAsJsonAsync("observation", precipitation)
        .Result;

    if (precipResponse.IsSuccessStatusCode)
    {
        Console.Write($"Posted Precipitation: Date {day:d} " +
            $"Zip: {zip} " +
            $"Type: {precipitation.WeatherType} " +
            $"Amount (in.): {precipitation.AmountInches}");
    }
}

List<int> PostTemp(string zip, DateTime day, HttpClient httpClient)
{
    var rand = new Random();
    var t1 = rand.Next(0, 100);
    var t2 = rand.Next(0, 100);
    var hiLoTemps = new List<int> { t1, t2 };
    hiLoTemps.Sort();

    var temperatureObservation = new TemperatureModel
    {
        TempLowF = hiLoTemps[0],
        TempHighF = hiLoTemps[1],
        ZipCode = zip,
        CreatedOn = day
    };

    var tempResponse = httpClient
        .PostAsJsonAsync("observation", temperatureObservation)
        .Result;

    if (tempResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Posted Temperature: Date: {day:d} " +
            $"Zip: {zip} " +
            $"Lo (F): {hiLoTemps[0]} " +
            $"Hi (F): {hiLoTemps[1]}");
    }
    else
    {
        Console.WriteLine(tempResponse.ToString());
    }

    return hiLoTemps;
}