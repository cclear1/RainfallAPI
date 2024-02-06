using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RainfallAPI.Models
{
    public class RainfallReading
    {
        public DateTime dateMeasured { get; set; }

        public decimal amountMeasured { get; set; }

        public override string ToString()
        {
            return $"RainfallReading {{DateMeasured: {dateMeasured}, AmountMeasured: {amountMeasured}}}";
        }
    }
}
