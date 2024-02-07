using System.Runtime.Serialization;

namespace RainfallAPI.Models
{
    public class RainfallReadingResponse
    {
        public List<RainfallReading> Readings { get; set; }

        public override string ToString()
        {
            return $"RainfallReadingResponse  {{Readings: {string.Join(", ", Readings)}}}";
        }
    }
}
