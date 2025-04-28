namespace ExchangeService.Services
{
    public interface IExchangeRateService
    {
        Task<decimal?> GetExchangeRateAsync(string inputCurrency, string outputCurrency);
    }
}