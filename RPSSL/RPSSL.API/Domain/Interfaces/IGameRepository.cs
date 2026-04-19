using RPSSL.API.Domain.Entities;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> CreateAsync(Game game);
        Task<IEnumerable<Game>> GetByPlayerIdAsync(Guid playerId);
    }
}

