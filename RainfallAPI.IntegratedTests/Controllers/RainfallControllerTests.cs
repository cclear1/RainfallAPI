using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace RainfallAPI.IntegratedTests.Controllers
{
    [TestFixture]
    public class RainfallControllerTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [Test]
        public async Task GetRainfallReading_CorrectParams_ReturnsData()
        {
            string stationId = "3680";
            var response = await _httpClient.GetAsync($"/rainfall/id/{stationId}/readings");
            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetRainfallReading_IncorrectParams_ReturnsBadRequest()
        {
            string stationId = "station";
            int count = 101;
            var response = await _httpClient.GetAsync($"/rainfall/id/{stationId}/readings?count={count}");
            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetRainfallReading_CorrectParamsNoReadings_ReturnsNotFound()
        {
            string stationId = "station";
            var response = await _httpClient.GetAsync($"/rainfall/id/{stationId}/readings");
            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}