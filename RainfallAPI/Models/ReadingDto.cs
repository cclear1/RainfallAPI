namespace RainfallAPI.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class ReadingDto
    {
        [JsonIgnore]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        [JsonIgnore]
        public string Measure { get; set; }
        public double Value { get; set; }
    }
}
