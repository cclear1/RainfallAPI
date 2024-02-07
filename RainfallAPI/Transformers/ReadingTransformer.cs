using RainfallAPI.Models;

namespace RainfallAPI.Transformers
{
    public class ReadingTransformer : ITransformer<ReadingDto, RainfallReading>
    {
        public RainfallReading Transform(ReadingDto input)
        {
            RainfallReading rainfallReadingDto = new RainfallReading
            {
                DateMeasured = input.DateTime,
                AmountMeasured = Convert.ToDecimal(input.Value)
            };

            return rainfallReadingDto;
        }
    }
}
