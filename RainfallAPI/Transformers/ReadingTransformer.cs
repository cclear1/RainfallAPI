using RainfallAPI.Models;

namespace RainfallAPI.Transformers
{
    public class ReadingTransformer : ITransformer<ReadingDto, RainfallReading>
    {
        public RainfallReading Transform(ReadingDto input)
        {
            RainfallReading rainfallReadingDto = new RainfallReading
            {
                dateMeasured = input.DateTime,
                amountMeasured = Convert.ToDecimal(input.Value)
            };

            return rainfallReadingDto;
        }
    }
}
