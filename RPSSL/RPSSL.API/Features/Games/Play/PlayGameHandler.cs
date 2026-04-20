using RPSSL.API.Domain.Entities;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Domain.ValueObjects;
using RPSSL.API.Infrastructure.Persistence.Configuration;

namespace RPSSL.API.Features.Games.Play
{
    public class PlayGameHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRandomNumberService _randomNumberService;
        private readonly IChoiceService _choiceService;
        private readonly ILogger<PlayGameHandler> _logger;

        public PlayGameHandler(
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            IRandomNumberService randomNumberService,
            IChoiceService choiceService,
            ILogger<PlayGameHandler> logger)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _randomNumberService = randomNumberService;
            _choiceService = choiceService;
            _logger = logger;
        }

        /// <summary>
        /// Resolves the player, generates a computer choice via the external random number service,
        /// creates and persists the game, and returns the result.
        /// </summary>
        public async Task<PlayGameResponse> Handle(PlayGameCommand request, CancellationToken ct)
        {
            _logger.LogInformation("PlayGame started. PlayerId: {PlayerId}, Choice: {Choice}",
                request.PlayerId, request.Choice);

            var player = request.PlayerId.HasValue
                ? await _playerRepository.GetByIdAsync(request.PlayerId.Value)
                : await _playerRepository.GetByIdAsync(SeedData.AnonymousPlayerId);

            if (player is null)
                throw new InvalidOperationException(
                    request.PlayerId.HasValue
                        ? $"Player with id '{request.PlayerId}' was not found."
                        : "Anonymous player not found.");

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

            var playerChoice = _choiceService.GetByRandomNumber(PositiveNumber.Create(request.Choice));
            var computerChoice = _choiceService.GetByRandomNumber(PositiveNumber.Create(randomNumber));

            var game = Game.Create(Guid.NewGuid(), player, playerChoice, computerChoice);
            await _gameRepository.CreateAsync(game);

            _logger.LogInformation("PlayGame completed. Result: {Result}", game.Result);

            return new PlayGameResponse
            {
                Results = game.Result.ToString().ToLower(),
                Player = (int)game.PlayerChoice,
                Computer = (int)game.ComputerChoice
            };
        }
    }
}
