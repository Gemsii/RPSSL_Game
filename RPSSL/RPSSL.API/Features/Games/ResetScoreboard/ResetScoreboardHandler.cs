using RPSSL.API.Domain.Interfaces;

namespace RPSSL.API.Features.Games.ResetScoreboard
{
    public class ResetScoreboardHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<ResetScoreboardHandler> _logger;

        public ResetScoreboardHandler(IGameRepository gameRepository, ILogger<ResetScoreboardHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        /// <summary>
        /// Removes all games belonging to the specified player.
        /// </summary>
        public async Task Handle(ResetScoreboardCommand command, CancellationToken ct)
        {
            _logger.LogInformation("ResetScoreboard started. PlayerId: {PlayerId}", command.PlayerId);

            await _gameRepository.DeleteByPlayerIdAsync(command.PlayerId, ct);

            _logger.LogInformation("ResetScoreboard completed. PlayerId: {PlayerId}", command.PlayerId);
        }
    }
}
