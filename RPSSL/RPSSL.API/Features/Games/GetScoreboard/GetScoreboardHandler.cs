using RPSSL.API.Domain.Interfaces;

namespace RPSSL.API.Features.Games.GetScoreboard
{
    public class GetScoreboardHandler
    {
        private const int PageSize = 10;

        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GetScoreboardHandler> _logger;

        public GetScoreboardHandler(IGameRepository gameRepository, ILogger<GetScoreboardHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        /// <summary>
        /// Returns a page of games played by the specified player. Page size is fixed at <see cref="PageSize"/>.
        /// </summary>
        public async Task<IEnumerable<ScoreboardEntryResponse>> Handle(GetScoreboardQuery query, CancellationToken ct)
        {
            _logger.LogInformation("GetScoreboard started. PlayerId: {PlayerId}, Page: {Page}", query.PlayerId, query.Page);

            var games = await _gameRepository.GetByPlayerIdAsync(query.PlayerId, query.Page, PageSize);

            return games.Select(g => new ScoreboardEntryResponse
            {
                Results = g.Result.ToString().ToLower(),
                PlayerChoice = (int)g.PlayerChoice,
                ComputerChoice = (int)g.ComputerChoice
            });
        }
    }
}
