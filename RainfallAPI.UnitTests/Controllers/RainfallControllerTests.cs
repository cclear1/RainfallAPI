using Microsoft.AspNetCore.Mvc;
using Moq;
using RainfallAPI.Controllers;
using RainfallAPI.Exceptions;
using RainfallAPI.Models;
using RainfallAPI.Services;
using Microsoft.Extensions.Logging;

namespace RainfallAPI.UnitTests.Controllers
{
    [TestFixture]
    public class RainfallControllerTests
    {
        [Test]
        public async Task GetRainfallReadingsByStationId_SuccessfulResponse_ReturnsOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RainfallController>>();
            var mockService = new Mock<IRainfallService>();

            var controller = new RainfallController(mockLogger.Object, mockService.Object);
            string stationId = "123";
            var expectedResponse = new RainfallReadingResponse();

            mockService.Setup(s => s.GetRainfallReadingsForStationAsync(stationId, It.IsAny<int>()))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.GetRainfallReadingsByStationId(stationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult.Value);
        }

        [Test]
        public async Task GetRainfallReadingsByStationId_ErrorRequestException_ReturnsStatusCodeAndError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RainfallController>>();
            var mockService = new Mock<IRainfallService>();

            var controller = new RainfallController(mockLogger.Object, mockService.Object);
            string stationId = "123";
            var expectedError = new Error { Message = "Test error message" };

            mockService.Setup(s => s.GetRainfallReadingsForStationAsync(stationId, It.IsAny<int>()))
                       .ThrowsAsync(new ErrorRequestException(404, expectedError));

            // Act
            var result = await controller.GetRainfallReadingsByStationId(stationId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(404, objectResult.StatusCode);
            Assert.AreEqual(expectedError, objectResult.Value);
        }
    }
}

