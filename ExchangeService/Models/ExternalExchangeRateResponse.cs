using System.Text.Json.Serialization;

namespace ExchangeService.Models
{
    public class ExternalExchangeRateResponse
    {
        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; } = new();
    }
}
