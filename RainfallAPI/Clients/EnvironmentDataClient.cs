namespace RainfallAPI.Clients
{
    public class EnvironmentDataClient : IEnvironmentDataClient
    {
        private readonly ILogger<EnvironmentDataClient> _logger;
        private readonly HttpClient _httpClient;

        private const string BaseUrl = "https://environment.data.gov.uk";
        private const string RainfallForStationEndpoint = "/flood-monitoring/id/stations/{0}/readings";

        public EnvironmentDataClient(ILogger<EnvironmentDataClient> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string?> GetRainfallReadingsForStation(string stationId, int? count)
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUrl);
            uriBuilder.Port = -1;
            uriBuilder.Path = string.Format(RainfallForStationEndpoint, stationId);
            
            string query = "_sorted";
            if (count.HasValue)
            {
                query += $"&_limit={count}"; 
            }
            uriBuilder.Query = query;

            string? responseData = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();
                responseData = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Error fetching rainfall reading for station: {}", ex.Message);
            }

            return responseData;
        }
    }
}
