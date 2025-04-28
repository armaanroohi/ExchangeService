using ExchangeService.Models;
using ExchangeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeServiceController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeServiceController> _logger;


        public ExchangeServiceController(IExchangeRateService exchangeRateService, ILogger<ExchangeServiceController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Convert([FromBody] ExchangeRequest request)
        {
            if (request.InputCurrency != "AUD" || request.OutputCurrency != "USD")
            {
                _logger.LogWarning("Unsupported currency conversion requested: {InputCurrency} to {OutputCurrency}",
                    request.InputCurrency, request.OutputCurrency);
                return BadRequest("Only AUD to USD conversion is supported.");
            }

            var rate = await _exchangeRateService.GetExchangeRateAsync(request.InputCurrency, request.OutputCurrency);

            if (rate == null)
            {
                _logger.LogWarning("Exchange rate not found for {InputCurrency} to {OutputCurrency}",
                    request.InputCurrency, request.OutputCurrency);

                return BadRequest($"The requested currency '{request.OutputCurrency}' is not supported.");
            }

            var response = new ExchangeResponse
            {
                Amount = request.Amount,
                InputCurrency = request.InputCurrency,
                OutputCurrency = request.OutputCurrency,
                Value = Math.Round(request.Amount * rate.Value, 2)
            };

            return Ok(response);
        }
    }
}
