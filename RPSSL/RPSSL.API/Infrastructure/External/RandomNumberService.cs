using Microsoft.Extensions.Options;
using RPSSL.API.Domain.Exceptions;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.External.Models;
using RPSSL.API.Infrastructure.External.Options;

namespace RPSSL.API.Infrastructure.External
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly HttpClient _httpClient;
        private readonly int _maxRetries;
        private readonly ILogger<RandomNumberService> _logger;

        public RandomNumberService(HttpClient httpClient, IOptions<CodeChallengeApiOptions> options, ILogger<RandomNumberService> logger)
        {
            _httpClient = httpClient;
            _maxRetries = options.Value.RetryCount;
            _logger = logger;
        }

        public async Task<int> GetRandomNumberAsync()
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var response = await _httpClient.GetAsync("random");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<RandomNumberResponse>();

                if (result is null)
                    throw new ExternalServiceUnavailableException();

                return result.RandomNumber;
            });
        }

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            for (int attempt = 1; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex) when (attempt < _maxRetries)
                {
                    _logger.LogWarning(ex, "Random number service failed on attempt {Attempt}/{MaxRetries}. Retrying in {Delay}s.",
                        attempt, _maxRetries, Math.Pow(2, attempt));
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Random number service failed on final attempt {Attempt}/{MaxRetries}.",
                        attempt, _maxRetries);
                }
            }

            throw new ExternalServiceUnavailableException();
        }
    }
}
