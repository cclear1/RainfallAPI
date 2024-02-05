using Newtonsoft.Json;
using RainfallAPI.Models.Dto;

namespace RainfallAPI.Clients
{
    public class EnvironmentDataClient : IEnvironmentDataClient
    {
        private readonly ILogger<EnvironmentDataClient> _logger;
        private readonly HttpClient _httpClient;

        private const string BASE_URL = "https://environment.data.gov.uk";
        private const string RAINFALL_FOR_STATION_ENDPOINT = "/flood-monitoring/id/stations/{0}/readings";

        public EnvironmentDataClient(ILogger<EnvironmentDataClient> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BASE_URL);
        }

        public async Task<List<ReadingDto>> GetRainfallReadingsForStation(string stationId, int? count)
        {
            UriBuilder uriBuilder = new UriBuilder(BASE_URL);
            uriBuilder.Port = -1;
            uriBuilder.Path = string.Format(RAINFALL_FOR_STATION_ENDPOINT, stationId);
            
            string query = "_sorted";
            if (count.HasValue)
            {
                query += $"&_limit={count}"; 
            }
            uriBuilder.Query = query;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseDto>(responseBody);
                return result.Items;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error fetching rainfall reading for station: {}", ex.Message);
                return new List<ReadingDto>();
            }
        }
    }
}
