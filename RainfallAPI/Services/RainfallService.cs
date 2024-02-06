using RainfallAPI.Clients;
using RainfallAPI.Controllers;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;
using RainfallAPI.Transformers;

namespace RainfallAPI.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly ILogger _logger;   
        private readonly IEnvironmentDataClient _environmentDataClient;
        private readonly ITransformer<ReadingDto, RainfallReading> _readingTransformer;

        public RainfallService(ILogger<RainfallController> logger, IEnvironmentDataClient environmentDataClient, ITransformer<ReadingDto, RainfallReading> readingTransformer) 
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
                throw new ErrorRequestException(404, new Error { message = $"No readings found for the specified stationId: {stationId}" });
            }

            return ConvertToRainfallReadingResponse(response);
        }

        private void ValidateParameters(string stationId, int count)
        {
            List<ErrorDetail> errorDetails = new List<ErrorDetail>();

            if (stationId == null)
            {
                errorDetails.Add(new ErrorDetail { message = "Field cannot be null", propertyName = "stationId"});
            }
            if (count > 100) 
            {
                errorDetails.Add(new ErrorDetail { message = "Field cannot be greater than 100", propertyName = "count" });
            }
            if (count < 0)
            {
                errorDetails.Add(new ErrorDetail { message = "Field cannot be negative", propertyName = "count" });
            }

            if (errorDetails.Count > 0)
            {
                throw new ErrorRequestException(400, new Error { message = "Invalid request", detail = errorDetails });
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
                readings = rainfallReadings
            };

            return rainfallReadingResponseDto;
        }

    }
}
