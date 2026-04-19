using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Features.Choices.GetChoices;

namespace RPSSL.API.Features.Choices.GetRandomChoice
{
    public class GetRandomChoiceHandler
    {
        private readonly IRandomNumberService _randomNumberService;
        private readonly ILogger<GetRandomChoiceHandler> _logger;

        public GetRandomChoiceHandler(IRandomNumberService randomNumberService, ILogger<GetRandomChoiceHandler> logger)
        {
            _randomNumberService = randomNumberService;
            _logger = logger;
        }

        public async Task<ChoiceResponse> Handle(GetRandomChoiceQuery query, CancellationToken ct)
        {
            _logger.LogInformation("GetRandomChoice started.");

            int randomNumber;
            try
            {
                randomNumber = await _randomNumberService.GetRandomNumberAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "External random number service failed. Falling back to Random.Shared.");
                randomNumber = Random.Shared.Next(1, 101);
            }

            var choiceId = ((randomNumber - 1) % 5) + 1;
            var choice = (Choice)choiceId;

            return new ChoiceResponse
            {
                Id = choiceId,
                Name = choice.ToString().ToLower()
            };
        }
    }
}
