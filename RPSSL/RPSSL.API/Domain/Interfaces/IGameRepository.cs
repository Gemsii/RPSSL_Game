using RPSSL.API.Domain.Entities;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IGameRepository
    {
        /// <summary>Persists a new game to the store.</summary>
        Task<Game> CreateAsync(Game game);

        /// <summary>Returns all games played by the specified player, ordered by most recent first.</summary>
        Task<IEnumerable<Game>> GetByPlayerIdAsync(Guid playerId, int page, int pageSize);

        /// <summary>Removes all games belonging to the specified player.</summary>
        Task DeleteByPlayerIdAsync(Guid playerId, CancellationToken ct);
    }
}

