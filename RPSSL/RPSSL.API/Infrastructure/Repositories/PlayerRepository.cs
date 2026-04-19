using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.Mappers;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using DomainEntities = RPSSL.API.Domain.Entities;

namespace RPSSL.API.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly InMemoryDbContext _context;

        public PlayerRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<DomainEntities.Player?> GetByIdAsync(Guid id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
            return player is null ? null : PlayerMapper.ToDomain(player);
        }

        public async Task<DomainEntities.Player?> GetByNameAsync(string name)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == name);
            return player is null ? null : PlayerMapper.ToDomain(player);
        }

        public async Task<DomainEntities.Player> CreateAsync(DomainEntities.Player player)
        {
            var persistence = PlayerMapper.ToPersistence(player);
            _context.Players.Add(persistence);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<IEnumerable<DomainEntities.Player>> GetScoreboardByPageAsync(int page, int pageSize)
        {
            var players = await _context.Players
                .Include(p => p.Games)
                .OrderByDescending(p => p.Games.Count(g => g.Result == (int)RPSSL.API.Domain.Enums.GameResult.Win))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return players.Select(PlayerMapper.ToDomain);
        }
    }
}

