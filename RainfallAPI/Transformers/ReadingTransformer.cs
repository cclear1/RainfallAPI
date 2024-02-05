using RainfallAPI.Models.Dto;
using RainfallAPI.Models.DTO;

namespace RainfallAPI.Transformers
{
    public class ReadingTransformer : ITransformer<ReadingDto, RainfallReadingDto>
    {
        public RainfallReadingDto Transform(ReadingDto input)
        {
            RainfallReadingDto rainfallReadingDto = new RainfallReadingDto
            {
                dateMeasured = input.DateTime,
                amountMeasured = Convert.ToDecimal(input.Value)
            };

            return rainfallReadingDto;
        }
    }
}
