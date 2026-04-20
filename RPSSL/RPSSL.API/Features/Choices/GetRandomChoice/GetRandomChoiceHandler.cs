using RPSSL.API.Domain.Exceptions;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Domain.ValueObjects;
using RPSSL.API.Features.Choices.GetChoices;

namespace RPSSL.API.Features.Choices.GetRandomChoice
{
    public class GetRandomChoiceHandler
    {
        private readonly IRandomNumberService _randomNumberService;
        private readonly IChoiceService _choiceService;
        private readonly ILogger<GetRandomChoiceHandler> _logger;

        public GetRandomChoiceHandler(IRandomNumberService randomNumberService, IChoiceService choiceService, ILogger<GetRandomChoiceHandler> logger)
        {
            _randomNumberService = randomNumberService;
            _choiceService = choiceService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches a random number from the external service and maps it to a game choice.
        /// Falls back to <see cref="Random.Shared"/> if the external service is unavailable.
        /// </summary>
        public async Task<ChoiceResponse> Handle(GetRandomChoiceQuery query, CancellationToken ct)
        {
            _logger.LogInformation("GetRandomChoice started.");

            int randomNumber;
            try
            {
                randomNumber = await _randomNumberService.GetRandomNumberAsync();
            }
            catch (ExternalServiceUnavailableException ex)
            {
                _logger.LogWarning(ex, "External random number service failed. Falling back to Random.Shared.");
                randomNumber = Random.Shared.Next(1, 101);
            }

            var choice = _choiceService.GetByRandomNumber(PositiveNumber.Create(randomNumber));

            return new ChoiceResponse
            {
                Id = (int)choice,
                Name = choice.ToString().ToLower()
            };
        }
    }
}
