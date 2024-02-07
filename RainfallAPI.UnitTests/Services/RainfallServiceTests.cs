using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using RainfallAPI.Clients;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;
using RainfallAPI.Services;
using RainfallAPI.Transformers;

namespace RainfallAPI.UnitTests.Services
{
    [TestFixture]
    public class RainfallServiceTests
    {
        private RainfallService _rainfallService;
        private Mock<IEnvironmentDataClient> _environmentDataClientMock;
        private Mock<ITransformer<ReadingDto, RainfallReading>> _readingTransformerMock;
        private Mock<ILogger<RainfallService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _environmentDataClientMock = new Mock<IEnvironmentDataClient>();
            _readingTransformerMock = new Mock<ITransformer<ReadingDto, RainfallReading>>();
            _loggerMock = new Mock<ILogger<RainfallService>>();
            _rainfallService = new RainfallService(_loggerMock.Object, _environmentDataClientMock.Object, _readingTransformerMock.Object);
        }

        [Test]
        public async Task GetRainfallReadingsForStationAsync_ValidParameters_ReturnsRainfallReading()
        {
            // Arrange
            DateTime expectedDateMeasured = new DateTime(2024, 1, 1, 1, 0, 0);
            decimal expectedAmountMeasured = 8.5m;

            string stationId = "123";
            int count = 1;

            var response = new List<ReadingDto> { new() { DateTime = expectedDateMeasured, Value = (double)expectedAmountMeasured } };
            _environmentDataClientMock.Setup(client => client.GetRainfallReadingsForStation(stationId, count)).ReturnsAsync(response);
            _readingTransformerMock.Setup(transformer => transformer.Transform(It.IsAny<ReadingDto>())).Returns(new RainfallReading
            {
                AmountMeasured = expectedAmountMeasured,
                DateMeasured = expectedDateMeasured
            });

            // Act
            var result = await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RainfallReadingResponse>(result);
            Assert.That(result.Readings.Count, Is.EqualTo(response.Count));
            RainfallReading rainfallReading = result.Readings[0];
            Assert.That(rainfallReading.AmountMeasured, Is.EqualTo(expectedAmountMeasured));
            Assert.That(rainfallReading.DateMeasured, Is.EqualTo(expectedDateMeasured));
        }

        [Test]
        public async Task GetRainfallReadingsForStationAsync_InvalidParameters_ReturnsErrorDetails()
        {
            // Arrange
            string stationId = "";
            int count = -1;
            ErrorDetail expectionErrorDetail1 = new() { Message = "Field cannot be null", PropertyName = "stationId" };
            ErrorDetail expectionErrorDetail2 = new() { Message = "Field cannot be negative", PropertyName = "count" };

            try
            {
                // Act
                var result = await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);
                Assert.Fail();
            }
            catch (ErrorRequestException ex)
            {
                // Assert
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                Assert.IsNotNull(ex.Error.Detail);
                List<ErrorDetail> errorDetails = ex.Error.Detail;
                Assert.That(errorDetails[0].Message, Is.EqualTo(expectionErrorDetail1.Message));
                Assert.That(errorDetails[0].PropertyName, Is.EqualTo(expectionErrorDetail1.PropertyName));
                Assert.That(errorDetails[1].Message, Is.EqualTo(expectionErrorDetail2.Message));
                Assert.That(errorDetails[1].PropertyName, Is.EqualTo(expectionErrorDetail2.PropertyName));
            }
        }

        [Test]
        public async Task GetNoRainfallReadingsForStationAsync_ValidParameters_ReturnsErrorResponse()
        {
            // Arrange
            string stationId = "123";
            int count = 1;
            Error expectedError = new() { Message = $"No readings found for the specified stationId: {stationId}" };

            var response = new List<ReadingDto>();
            _environmentDataClientMock.Setup(client => client.GetRainfallReadingsForStation(stationId, count)).ReturnsAsync(response);

            try
            {
                // Act
                var result = await _rainfallService.GetRainfallReadingsForStationAsync(stationId, count);
                Assert.Fail();
            }
            catch (ErrorRequestException ex)
            {
                // Assert
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                Assert.That(ex.Error.Message, Is.EqualTo(expectedError.Message));
            }
        }
    }
}
