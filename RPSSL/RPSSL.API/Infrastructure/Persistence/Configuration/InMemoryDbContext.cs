using Microsoft.EntityFrameworkCore;
using RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.API.Infrastructure.Persistence.Configuration
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Game> Games => Set<Game>();

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            if (Players.Any())
                return;

            Players.AddRange(
                new Player { Id = SeedData.ComputerPlayerId, Name = SeedData.ComputerPlayerName },
                new Player { Id = SeedData.AnonymousPlayerId, Name = SeedData.AnonymousPlayerName }
            );

            SaveChanges();
        }
    }
}
