using RainfallAPI.Models;
using RainfallAPI.Models.DTO;

namespace RainfallAPI.Services
{
    public interface IRainfallService
    {
        Task<RainfallReadingResponseDto> GetRainfallReadingsForStationAsync(string stationId, int count);
    }
}
