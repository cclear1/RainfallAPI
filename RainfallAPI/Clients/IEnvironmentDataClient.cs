using RainfallAPI.Models;

namespace RainfallAPI.Clients
{
    public interface IEnvironmentDataClient
    {

        public Task<List<ReadingDto>> GetRainfallReadingsForStation(string stationId, int? count);
    }
}
