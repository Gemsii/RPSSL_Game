using RPSSL.API.Domain.Entities;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        /// <summary>Returns the player with the given id, or <c>null</c> if not found.</summary>
        Task<Player?> GetByIdAsync(Guid id);

        /// <summary>Returns the player with the given name, or <c>null</c> if not found.</summary>
        Task<Player?> GetByNameAsync(string name);

        /// <summary>Persists a new player to the store.</summary>
        Task<Player> CreateAsync(Player player);

        /// <summary>Returns a paged list of players ordered by score.</summary>
        Task<IEnumerable<Player>> GetScoreboardByPageAsync(int page, int pageSize);
    }
}

