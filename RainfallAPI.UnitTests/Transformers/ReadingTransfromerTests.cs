using NUnit.Framework;
using RainfallAPI.Models;
using RainfallAPI.Transformers;

namespace RainfallAPI.UnitTests.Transformers
{
    [TestFixture]
    public class ReadingTransformerTest
    {
        [Test]
        public void Transform_ReturnsCorrectRainfallReading()
        {
            // Arrange
            DateTime expectedDateMeasured = new DateTime(2024, 1, 1, 1, 0, 0);
            decimal expectedAmountMeasured = 8.5m;
            var transformer = new ReadingTransformer();
            var inputDto = new ReadingDto { DateTime = expectedDateMeasured, Value = (double)expectedAmountMeasured };

            // Act
            var result = transformer.Transform(inputDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DateMeasured, Is.EqualTo(expectedDateMeasured));
            Assert.That(result.AmountMeasured, Is.EqualTo(expectedAmountMeasured));
        }
    }
}
