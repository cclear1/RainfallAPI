using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;
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

        [HttpGet("/rainfall/id/{stationId}/readings")]
        [ProducesResponseType(typeof(RainfallReadingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRainfallReadingsByStationId(string stationId, [FromQuery] int count = 10)
        {
            try
            {
                _logger.LogInformation("Getting rainfall readings for station: {}", stationId);
                var response = await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);

                return Ok(response);
            }
            catch (ErrorRequestException ex)
            {
                _logger.LogError(ex, "Error fetching data from public API");
                // Set null status code to 500
                int statusCode = ex.StatusCode.HasValue ? (int)ex.StatusCode : 500; 
                return StatusCode(statusCode, ex.Error);
            }

        }
    }
}
