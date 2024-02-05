using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RainfallAPI.Models.DTO
{
    public class RainfallReadingDto
    {
        public DateTime dateMeasured { get; set; }

        [DataMember]
        public decimal amountMeasured { get; set; }

        public override string ToString()
        {
            return $"RainfallReading {{DateMeasured: {dateMeasured}, AmountMeasured: {amountMeasured}}}";
        }
    }
}
