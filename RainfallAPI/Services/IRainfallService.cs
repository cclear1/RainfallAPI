using RainfallAPI.Models;

namespace RainfallAPI.Services
{
    public interface IRainfallService
    {
        Task<RainfallReadingResponse> GetRainfallReadingsForStationAsync(string stationId, int count);
    }
}
