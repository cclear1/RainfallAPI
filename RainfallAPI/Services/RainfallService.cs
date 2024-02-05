using RainfallAPI.Clients;
using RainfallAPI.Controllers;
using RainfallAPI.Models.Dto;
using RainfallAPI.Models.DTO;
using RainfallAPI.Transformers;

namespace RainfallAPI.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly ILogger _logger;   
        private readonly IEnvironmentDataClient _environmentDataClient;
        private readonly ITransformer<ReadingDto, RainfallReadingDto> _readingTransformer;

        public RainfallService(ILogger<RainfallController> logger, IEnvironmentDataClient environmentDataClient, ITransformer<ReadingDto, RainfallReadingDto> readingTransformer) 
        { 
            _logger = logger;
            _environmentDataClient = environmentDataClient;
            _readingTransformer = readingTransformer;
        }

        public async Task<RainfallReadingResponseDto> GetRainfallReadingsForStationAsync(string stationId, int count)
        {
            List<ReadingDto> response = await _environmentDataClient.GetRainfallReadingsForStation(stationId, count);
            _logger.LogInformation("Reponse: {}", response);
            return ConvertToRainfallReadingResponse(response);
        }

        private RainfallReadingResponseDto ConvertToRainfallReadingResponse(List<ReadingDto> readingDtos)
        {
            var rainfallReadings = new List<RainfallReadingDto>();
            foreach (var reading in readingDtos)
            {
                _logger.LogInformation($"Reading {reading.Value}");
                rainfallReadings.Add(_readingTransformer.Transform(reading)); 
                _logger.LogInformation($"Reading transformed {_readingTransformer.Transform(reading)}");
            }

            var rainfallReadingResponseDto = new RainfallReadingResponseDto
            {
                readings = rainfallReadings
            };
            _logger.LogInformation($"RainfallReadingResponse: {rainfallReadingResponseDto}");

            return rainfallReadingResponseDto;
        }

    }
}
