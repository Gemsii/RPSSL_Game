using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RPSSL.API.Domain.Exceptions;
using RPSSL.API.Infrastructure.External;
using RPSSL.API.Infrastructure.External.Options;

namespace RPSSL.Tests.Infrastructure
{
    public class RandomNumberServiceTests
    {
        private static RandomNumberService CreateSut(HttpMessageHandler handler, int retryCount = 1)
        {
            var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.local/") };
            var options = Options.Create(new CodeChallengeApiOptions { RetryCount = retryCount });
            return new RandomNumberService(client, options, NullLogger<RandomNumberService>.Instance);
        }

        private static HttpMessageHandler RespondWith(int statusCode, string? jsonBody = null)
        {
            return new FakeHttpMessageHandler(
                new HttpResponseMessage((HttpStatusCode)statusCode)
                {
                    Content = jsonBody is null
                        ? new StringContent(string.Empty)
                        : new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json")
                });
        }

        private static HttpMessageHandler FailThenSucceed(int failCount, string successJson)
        {
            return new FakeHttpMessageHandler(failCount, successJson);
        }

        // Happy path

        [Fact]
        public async Task GetRandomNumberAsync_ReturnsNumber_WhenApiRespondsSuccessfully()
        {
            var json = JsonSerializer.Serialize(new { random_number = 42 });
            var sut = CreateSut(RespondWith(200, json));

            var result = await sut.GetRandomNumberAsync();

            Assert.Equal(42, result);
        }

        // Failure paths

        [Fact]
        public async Task GetRandomNumberAsync_Throws_WhenApiReturnsNullBody()
        {
            var json = "null";
            var sut = CreateSut(RespondWith(200, json));

            await Assert.ThrowsAsync<ExternalServiceUnavailableException>(
                () => sut.GetRandomNumberAsync());
        }

        [Fact]
        public async Task GetRandomNumberAsync_Throws_WhenApiReturnsNonSuccessStatus()
        {
            // retryCount = 1 → no delay, throws ExternalServiceUnavailableException immediately
            var sut = CreateSut(RespondWith(500), retryCount: 1);

            await Assert.ThrowsAsync<ExternalServiceUnavailableException>(
                () => sut.GetRandomNumberAsync());
        }

        // Retry logic

        [Fact]
        public async Task GetRandomNumberAsync_ReturnsNumber_WhenFirstAttemptFailsButSecondSucceeds()
        {
            // retryCount = 2: first attempt fails, second succeeds – delay = 2^1 = 2s
            // Note: this test waits ~2s due to exponential backoff
            var json = JsonSerializer.Serialize(new { random_number = 77 });
            var sut = CreateSut(FailThenSucceed(failCount: 1, json), retryCount: 2);
            var result = await sut.GetRandomNumberAsync();

            Assert.Equal(77, result);
        }

        // Helpers

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;
            private int _failsRemaining;
            private readonly string? _successJson;

            // Always returns the same response
            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
                _failsRemaining = 0;
            }

            // Returns failure failCount times, then succeeds with successJson
            public FakeHttpMessageHandler(int failCount, string successJson)
            {
                _response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                _failsRemaining = failCount;
                _successJson = successJson;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (_failsRemaining > 0)
                {
                    _failsRemaining--;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                }

                if (_successJson is not null)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(_successJson, System.Text.Encoding.UTF8, "application/json")
                    });
                }

                return Task.FromResult(_response);
            }
        }
    }
}
