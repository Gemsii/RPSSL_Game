using RPSSL.API.Domain.Entities;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(Guid id);
        Task<Player?> GetByNameAsync(string name);
        Task<Player> CreateAsync(Player player);
        Task<IEnumerable<Player>> GetScoreboardByPageAsync(int page, int pageSize);
    }
}

