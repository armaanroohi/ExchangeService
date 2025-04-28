using ExchangeService.Controllers;
using ExchangeService.Models;
using ExchangeService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeService.Tests
{
    public class ExchangeServiceControllerTests
    {
        [Fact]
        public async Task Convert_ReturnsCorrectExchangeValue_WhenValidCurrenciesProvided()
        {
            // Arrange
            var mockService = new Mock<IExchangeRateService>();
            mockService.Setup(service => service.GetExchangeRateAsync("AUD", "USD"))
                       .ReturnsAsync(0.656m); // because 5 * 0.656 = 3.28

            var mockLogger = new Mock<ILogger<ExchangeServiceController>>();

            var controller = new ExchangeServiceController(mockService.Object, mockLogger.Object);

            var request = new ExchangeRequest
            {
                Amount = 5,
                InputCurrency = "AUD",
                OutputCurrency = "USD"
            };

            // Act
            var result = await controller.Convert(request) as OkObjectResult;
            var response = result?.Value as ExchangeResponse;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(5, response.Amount);
            Assert.Equal("AUD", response.InputCurrency);
            Assert.Equal("USD", response.OutputCurrency);
            Assert.Equal(3.28m, response.Value); 
        }
    }
}
