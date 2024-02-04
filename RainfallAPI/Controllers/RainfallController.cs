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
        [Route("id/{station}/readings")]
        public String GetRainfallReadingsByStationId(String station)
        {
            _logger.LogInformation("Getting rainfall readings for station: {}", station);
            return _rainfallService.GetRainfallReadings(station);
        }
    }
}
