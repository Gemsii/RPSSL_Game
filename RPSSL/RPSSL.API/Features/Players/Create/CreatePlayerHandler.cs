using RPSSL.API.Domain.Entities;
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

        public async Task<PlayerResponse?> Handle(CreatePlayerCommand request, CancellationToken ct)
        {
            _logger.LogInformation("CreatePlayer started. Name: {Name}", request.Name);

            var playerName = PlayerName.Create(request.Name);

            var existing = await _playerRepository.GetByNameAsync(playerName.Value);
            if (existing is not null)
            {
                _logger.LogWarning("Player with name '{Name}' already exists.", request.Name);
                return null;
            }

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
