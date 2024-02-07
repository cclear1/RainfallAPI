using RainfallAPI.Exceptions;
using RainfallAPI.Controllers;
using RainfallAPI.Models;
using RainfallAPI.Clients;
using RainfallAPI.Transformers;

namespace RainfallAPI.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly ILogger _logger;
        private readonly IEnvironmentDataClient _environmentDataClient;
        private readonly ITransformer<ReadingDto, RainfallReading> _readingTransformer;

        public RainfallService(ILogger<RainfallService> logger, IEnvironmentDataClient environmentDataClient, ITransformer<ReadingDto, RainfallReading> readingTransformer)
        {
            _logger = logger;
            _environmentDataClient = environmentDataClient;
            _readingTransformer = readingTransformer;
        }

        public async Task<RainfallReadingResponse> GetRainfallReadingsForStationAsync(string stationId, int count)
        {
            ValidateParameters(stationId, count);

            var response = await _environmentDataClient.GetRainfallReadingsForStation(stationId, count);
            _logger.LogInformation("Reponse: {}", response);

            // check for no response
            if (response == null || response.Count == 0)
            {
                throw new ErrorRequestException(404, new Error { Message = $"No readings found for the specified stationId: {stationId}" });
            }

            return ConvertToRainfallReadingResponse(response);
        }

        private void ValidateParameters(string stationId, int count)
        {
            List<ErrorDetail> errorDetails = new List<ErrorDetail>();

            if (stationId == null || stationId == "")
            {
                errorDetails.Add(new ErrorDetail { Message = "Field cannot be null", PropertyName = "stationId" });
            }
            if (count > 100)
            {
                errorDetails.Add(new ErrorDetail { Message = "Field cannot be greater than 100", PropertyName = "count" });
            }
            if (count < 0)
            {
                errorDetails.Add(new ErrorDetail { Message = "Field cannot be negative", PropertyName = "count" });
            }

            if (errorDetails.Count > 0)
            {
                throw new ErrorRequestException(400, new Error { Message = "Invalid request", Detail = errorDetails });
            }
        }

        private RainfallReadingResponse ConvertToRainfallReadingResponse(List<ReadingDto> readingDtos)
        {
            var rainfallReadings = new List<RainfallReading>();
            // transform received dto to required rainfall reading
            foreach (var reading in readingDtos)
            {
                rainfallReadings.Add(_readingTransformer.Transform(reading));
            }

            var rainfallReadingResponseDto = new RainfallReadingResponse
            {
                Readings = rainfallReadings
            };

            return rainfallReadingResponseDto;
        }

    }
}
