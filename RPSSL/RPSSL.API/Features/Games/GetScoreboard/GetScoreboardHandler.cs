using RPSSL.API.Domain.Interfaces;

namespace RPSSL.API.Features.Games.GetScoreboard
{
    public class GetScoreboardHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GetScoreboardHandler> _logger;

        public GetScoreboardHandler(IGameRepository gameRepository, ILogger<GetScoreboardHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ScoreboardEntryResponse>> Handle(GetScoreboardQuery query, CancellationToken ct)
        {
            _logger.LogInformation("GetScoreboard started. PlayerId: {PlayerId}", query.PlayerId);

            var games = await _gameRepository.GetByPlayerIdAsync(query.PlayerId);

            return games.Select(g => new ScoreboardEntryResponse
            {
                Results = g.Result.ToString().ToLower(),
                PlayerChoice = (int)g.PlayerChoice,
                ComputerChoice = (int)g.ComputerChoice
            });
        }
    }
}
