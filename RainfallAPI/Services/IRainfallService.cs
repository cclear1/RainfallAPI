namespace RainfallAPI.Services
{
    public interface IRainfallService
    {
        Task<string> GetRainfallReadingsForStationAsync(string stationId, int count);
    }
}
