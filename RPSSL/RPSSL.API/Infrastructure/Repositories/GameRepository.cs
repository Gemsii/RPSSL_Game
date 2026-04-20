using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.Mappers;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using DomainEntities = RPSSL.API.Domain.Entities;

namespace RPSSL.API.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly InMemoryDbContext _context;

        public GameRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<DomainEntities.Game> CreateAsync(DomainEntities.Game game)
        {
            var persistence = GameMapper.ToPersistence(game);
            _context.Games.Add(persistence);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<IEnumerable<DomainEntities.Game>> GetByPlayerIdAsync(Guid playerId, int page, int pageSize)
        {
            var games = await _context.Games
                .Include(g => g.Player)
                .Where(g => g.PlayerId == playerId)
                .OrderBy(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return games.Select(GameMapper.ToDomain);
        }

        public async Task DeleteByPlayerIdAsync(Guid playerId, CancellationToken ct)
        {
            var games = _context.Games.Where(g => g.PlayerId == playerId);
            _context.Games.RemoveRange(games);
            await _context.SaveChangesAsync(ct);
        }
    }
}
