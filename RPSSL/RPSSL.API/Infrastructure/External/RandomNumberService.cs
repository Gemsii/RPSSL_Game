using Microsoft.Extensions.Options;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.External.Models;
using RPSSL.API.Infrastructure.External.Options;
using System.Net.Http.Json;

namespace RPSSL.API.Infrastructure.External
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly HttpClient _httpClient;
        private readonly int _maxRetries;

        public RandomNumberService(HttpClient httpClient, IOptions<CodeChallengeApiOptions> options)
        {
            _httpClient = httpClient;
            _maxRetries = options.Value.RetryCount;
        }

        public async Task<int> GetRandomNumberAsync()
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var response = await _httpClient.GetAsync("random");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<RandomNumberResponse>();

                return result?.RandomNumber
                    ?? throw new InvalidOperationException("Failed to retrieve random number from external service.");
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
                catch when (attempt < _maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
            }

            throw new InvalidOperationException("External random number service is unavailable after multiple retries.");
        }
    }
}
