namespace CloudWeather.Report.BusinessLogic;

using CloudWeather.Report.DataAccess;

public interface IWeatherReportAggregator
{
    public Task<WeatherReport> BuildReport(string zip, int days);
}
