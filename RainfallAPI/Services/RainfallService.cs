using RainfallAPI.Clients;
using RainfallAPI.Controllers;

namespace RainfallAPI.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly ILogger _logger;   
        private readonly IEnvironmentDataClient _environmentDataClient;

        public RainfallService(ILogger<RainfallController> logger, IEnvironmentDataClient environmentDataClient) 
        { 
            _logger = logger;
            _environmentDataClient = environmentDataClient;
        }

        public async Task<string> GetRainfallReadingsForStationAsync(string stationId, int count)
        {
            string? response = await _environmentDataClient.GetRainfallReadingsForStation(stationId, count);
            _logger.LogInformation("Reponse: {}", response);
            return stationId;
        }

    }
}
