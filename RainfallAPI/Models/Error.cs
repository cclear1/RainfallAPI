namespace RainfallAPI.Models
{
    public class Error
    {
        public string message { get; set; }
        public List<ErrorDetail>? detail { get; set; }
    }
}
