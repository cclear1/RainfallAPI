using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Models.DTO;
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
        public async Task<ActionResult<RainfallReadingResponseDto>> GetRainfallReadingsByStationId(string stationId, [FromQuery] int count = 10)
        {
            _logger.LogInformation("Getting rainfall readings for station: {}", stationId);
            var response = await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);
            _logger.LogInformation(response.ToString());


            return Ok(response); 
        }
    }
}
