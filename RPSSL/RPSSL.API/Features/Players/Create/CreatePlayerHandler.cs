using RPSSL.API.Domain.Entities;
using RPSSL.API.Domain.Exceptions;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.API.Features.Players.Create
{
    public class CreatePlayerHandler
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<CreatePlayerHandler> _logger;

        public CreatePlayerHandler(IPlayerRepository playerRepository, ILogger<CreatePlayerHandler> logger)
        {
            _playerRepository = playerRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new player if a player with the same name does not already exist.
        /// Throws <see cref="PlayerAlreadyExistsException"/> if the name is taken.
        /// </summary>
        public async Task<PlayerResponse> Handle(CreatePlayerCommand request, CancellationToken ct)
        {
            _logger.LogInformation("CreatePlayer started. Name: {Name}", request.Name);

            var playerName = PlayerName.Create(request.Name);

            var existing = await _playerRepository.GetByNameAsync(playerName.Value);
            if (existing is not null)
                throw new PlayerAlreadyExistsException(request.Name);

            var player = Player.Create(Guid.NewGuid(), playerName);
            await _playerRepository.CreateAsync(player);

            _logger.LogInformation("Player '{Name}' created successfully.", request.Name);

            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name.Value
            };
        }
    }
}
