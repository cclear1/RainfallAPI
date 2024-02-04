namespace RainfallAPI.Clients
{
    public interface IEnvironmentDataClient
    {

        public Task<string?> GetRainfallReadingsForStation(string stationId, int? count);
    }
}
