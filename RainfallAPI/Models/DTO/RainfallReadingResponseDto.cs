using System.Runtime.Serialization;

namespace RainfallAPI.Models.DTO
{
    public class RainfallReadingResponseDto
    {
        public List<RainfallReadingDto> readings { get; set; }

        public override string ToString()
        {
            return $"RainfallReadingResponse  {{Readings: {string.Join(", ", readings)}}}";
        }
    }
}
