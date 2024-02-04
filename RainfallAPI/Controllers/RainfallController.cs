using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Services;

namespace RainfallAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {

        private readonly ILogger<RainfallController> _logger;
        private readonly IRainfallService _rainfallService;

        public RainfallController(ILogger<RainfallController> logger, IRainfallService rainfallService)
        {
            _logger = logger;
            _rainfallService = rainfallService;
        }

        [HttpGet]
        [Route("id/{stationId}/readings")]
        public async Task<string> GetRainfallReadingsByStationId(string stationId, [FromQuery] int count)
        {
            _logger.LogInformation("Getting rainfall readings for station: {}", stationId);
            return await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);
        }
    }
}
