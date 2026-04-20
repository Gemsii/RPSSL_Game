using RPSSL.API.Domain.Enums;

namespace RPSSL.API.Features.Choices.GetChoices
{
    public class GetChoicesHandler
    {
        private readonly ILogger<GetChoicesHandler> _logger;

        public GetChoicesHandler(ILogger<GetChoicesHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns all available game choices (Rock, Paper, Scissors, Lizard, Spock).
        /// </summary>
        public Task<List<ChoiceResponse>> Handle(GetChoicesQuery query, CancellationToken ct)
        {
            _logger.LogInformation("GetChoices started.");

            var choices = Enum.GetValues<Choice>()
                .Select(c => new ChoiceResponse
                {
                    Id = (int)c,
                    Name = c.ToString().ToLower()
                })
                .ToList();

            return Task.FromResult(choices);
        }
    }
}
