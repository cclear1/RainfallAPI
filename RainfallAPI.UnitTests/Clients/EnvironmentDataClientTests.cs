using Moq;
using System.Net;
using RainfallAPI.Clients;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Newtonsoft.Json;

namespace RainfallAPI.UnitTests.Clients
{
    [TestFixture]
    public class EnvironmentDataClientTests
    {
        [Test]
        public async Task GetRainfallReadingsForStation_SuccessfulResponse_ReturnsRainfallReadings()
        {
            // Arrange
            DateTime expectedDateMeasured = new DateTime(2024, 1, 1, 1, 0, 0);
            double expectedAmountMeasured = 8.5;
            List<ReadingDto> readings = new List<ReadingDto> { new ReadingDto { DateTime = expectedDateMeasured, Value = expectedAmountMeasured } };

            var loggerMock = new Mock<ILogger<EnvironmentDataClient>>();

            // mock HttpMessageHandler
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonConvert.SerializeObject(new ResponseDto
                           {
                               Items = readings
                           }))
                       });

            var httpClient = new HttpClient(handlerMock.Object);
            var client = new EnvironmentDataClient(loggerMock.Object, httpClient);

            // Act
            var result = await client.GetRainfallReadingsForStation("123", 10);

            // Assert
            Assert.NotNull(result);
            Assert.That(result[0].DateTime, Is.EqualTo(expectedDateMeasured));
            Assert.That(result[0].Value, Is.EqualTo(expectedAmountMeasured));
        }

        [Test]
        public async Task GetRainfallReadingsForStation_ErrorResponse_Throws500Exception()
        {
            // Arrange
            DateTime expectedDateMeasured = new DateTime(2024, 1, 1, 1, 0, 0);
            double expectedAmountMeasured = 8.5;
            List<ReadingDto> readings = new List<ReadingDto> { new ReadingDto { DateTime = expectedDateMeasured, Value = expectedAmountMeasured } };

            var loggerMock = new Mock<ILogger<EnvironmentDataClient>>();

            // mock HttpMessageHandler
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.InternalServerError
                       });

            var httpClient = new HttpClient(handlerMock.Object);
            var client = new EnvironmentDataClient(loggerMock.Object, httpClient);

            try
            {
                // Act
                var result = await client.GetRainfallReadingsForStation("123", 10);
                Assert.Fail();
            }
            catch (ErrorRequestException ex)
            {
                // Assert
                Assert.That(ex.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            }
        }

        [Test]
        public async Task GetRainfallReadingsForStation_EmptyResponse_Throws404Exception()
        {
            // Arrange
            DateTime expectedDateMeasured = new DateTime(2024, 1, 1, 1, 0, 0);
            double expectedAmountMeasured = 8.5;
            List<ReadingDto> readings = new List<ReadingDto> { new ReadingDto { DateTime = expectedDateMeasured, Value = expectedAmountMeasured } };

            var loggerMock = new Mock<ILogger<EnvironmentDataClient>>();

            // mock HttpMessageHandler
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.BadRequest
                       });

            var httpClient = new HttpClient(handlerMock.Object);
            var client = new EnvironmentDataClient(loggerMock.Object, httpClient);

            try
            {
                // Act
                var result = await client.GetRainfallReadingsForStation("123", 10);
                Assert.Fail();
            }
            catch (ErrorRequestException ex)
            {
                // Assert
                Assert.That(ex.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            }
        }
    }
}
