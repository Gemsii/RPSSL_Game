using Microsoft.EntityFrameworkCore;
using RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.API.Infrastructure.Persistence.Configuration
{
    public class RpsslDbContext : DbContext
    {
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Game> Games => Set<Game>();

        public RpsslDbContext(DbContextOptions<RpsslDbContext> options) : base(options)
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
            // Apply pending EF Core migrations (creates or updates the database schema)
            Database.Migrate();

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
