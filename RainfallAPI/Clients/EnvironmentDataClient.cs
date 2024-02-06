using Newtonsoft.Json;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;

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
            try
            {
                var response = await _httpClient.GetAsync(BuildUri(stationId, count));

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDto>(responseData);
                    return result.Items;
                }
                else
                {
                    var errorData = await response.Content.ReadAsStringAsync();
                    throw new ErrorRequestException((int)response.StatusCode, new Error { message = errorData });
                }
            }
            catch (HttpRequestException ex)
            {
                var error = new Error { message = ex.Message };
                throw new ErrorRequestException((int)ex.StatusCode, error);
            }
        }

        private Uri BuildUri(string stationId, int? count)
        {
            UriBuilder uriBuilder = new UriBuilder(BASE_URL);
            // remove default port and append endpoint path
            uriBuilder.Port = -1;
            uriBuilder.Path = string.Format(RAINFALL_FOR_STATION_ENDPOINT, stationId);

            // add query params
            string query = "_sorted";
            if (count.HasValue)
            {
                query += $"&_limit={count}";
            }
            uriBuilder.Query = query;

            return uriBuilder.Uri;
        }

    }
}
