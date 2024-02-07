using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RainfallAPI.Models
{
    public class RainfallReading
    {
        public DateTime DateMeasured { get; set; }

        public decimal AmountMeasured { get; set; }

        public override string ToString()
        {
            return $"RainfallReading {{DateMeasured: {DateMeasured}, AmountMeasured: {AmountMeasured}}}";
        }
    }
}
