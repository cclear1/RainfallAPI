using RainfallAPI.Models.Dto;

namespace RainfallAPI.Clients
{
    public interface IEnvironmentDataClient
    {

        public Task<List<ReadingDto>> GetRainfallReadingsForStation(string stationId, int? count);
    }
}
