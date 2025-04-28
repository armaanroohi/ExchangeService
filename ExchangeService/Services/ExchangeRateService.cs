using ExchangeService.Models;
using System.Text.Json;

namespace ExchangeService.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExchangeRateService> _logger;


        public ExchangeRateService(HttpClient httpClient, IConfiguration configuration, ILogger<ExchangeRateService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<decimal?> GetExchangeRateAsync(string inputCurrency, string outputCurrency)
        {
            var apiKey = _configuration["ExchangeRateApi:ApiKey"];
            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{inputCurrency}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rateResponse = JsonSerializer.Deserialize<ExternalExchangeRateResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (rateResponse?.ConversionRates != null && rateResponse.ConversionRates.TryGetValue(outputCurrency, out var rate))
            {
                return rate;
            }

            _logger.LogWarning("Exchange rate not found: {InputCurrency} to {OutputCurrency}", inputCurrency, outputCurrency);
            return null;
        }
    }
}