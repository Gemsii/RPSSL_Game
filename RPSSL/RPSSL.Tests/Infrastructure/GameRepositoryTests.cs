using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Entities;
using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.ValueObjects;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using RPSSL.API.Infrastructure.Repositories;
using PersistenceEntities = RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.Tests.Infrastructure
{
    public class GameRepositoryTests : IDisposable
    {
        private readonly InMemoryDbContext _context;
        private readonly GameRepository _sut;

        private static readonly Guid Player1Id = Guid.NewGuid();
        private static readonly Guid Player2Id = Guid.NewGuid();

        public GameRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test class instance
                .Options;

            _context = new InMemoryDbContext(options);
            _sut = new GameRepository(_context);

            SeedPlayers();
        }

        private void SeedPlayers()
        {
            _context.Players.AddRange(
                new PersistenceEntities.Player { Id = Player1Id, Name = "Alice" },
                new PersistenceEntities.Player { Id = Player2Id, Name = "Bob" }
            );
            _context.SaveChanges();
        }

        private static Player DomainPlayer(Guid id, string name) =>
            Player.Create(id, PlayerName.Create(name));

        // CreateAsync

        [Fact]
        public async Task CreateAsync_PersistsGame_CanBeRetrievedAfterwards()
        {
            var player = DomainPlayer(Player1Id, "Alice");
            var game = Game.Create(Guid.NewGuid(), player, Choice.Rock, Choice.Scissors);

            await _sut.CreateAsync(game);

            var stored = _context.Games.Single(g => g.Id == game.Id);
            Assert.Equal((int)Choice.Rock, stored.PlayerChoice);
            Assert.Equal((int)Choice.Scissors, stored.ComputerChoice);
            Assert.Equal((int)GameResult.Win, stored.Result);
        }

        // GetByPlayerIdAsync

        [Fact]
        public async Task GetByPlayerIdAsync_ReturnsOnlyGamesForRequestedPlayer()
        {
            var player1 = DomainPlayer(Player1Id, "Alice");
            var player2 = DomainPlayer(Player2Id, "Bob");

            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player1, Choice.Rock, Choice.Scissors));
            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player2, Choice.Paper, Choice.Rock));

            var result = (await _sut.GetByPlayerIdAsync(Player1Id, page: 1, pageSize: 10)).ToList();

            Assert.Single(result);
            Assert.Equal(Choice.Rock, result[0].PlayerChoice);
        }

        [Fact]
        public async Task GetByPlayerIdAsync_RespectsPageSize()
        {
            var player = DomainPlayer(Player1Id, "Alice");

            for (var i = 0; i < 15; i++)
                await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player, Choice.Rock, Choice.Paper));

            var page1 = (await _sut.GetByPlayerIdAsync(Player1Id, page: 1, pageSize: 10)).ToList();
            var page2 = (await _sut.GetByPlayerIdAsync(Player1Id, page: 2, pageSize: 10)).ToList();

            Assert.Equal(10, page1.Count);
            Assert.Equal(5, page2.Count);
        }

        [Fact]
        public async Task GetByPlayerIdAsync_MapsResultCorrectly()
        {
            var player = DomainPlayer(Player1Id, "Alice");
            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player, Choice.Spock, Choice.Rock)); // Win

            var result = (await _sut.GetByPlayerIdAsync(Player1Id, page: 1, pageSize: 10)).Single();

            Assert.Equal(Choice.Spock, result.PlayerChoice);
            Assert.Equal(Choice.Rock, result.ComputerChoice);
            Assert.Equal(GameResult.Win, result.Result);
        }

        // DeleteByPlayerIdAsync

        [Fact]
        public async Task DeleteByPlayerIdAsync_RemovesAllGamesForPlayer()
        {
            var player = DomainPlayer(Player1Id, "Alice");
            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player, Choice.Rock, Choice.Paper));
            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player, Choice.Scissors, Choice.Rock));

            await _sut.DeleteByPlayerIdAsync(Player1Id, CancellationToken.None);

            Assert.Empty(_context.Games.Where(g => g.PlayerId == Player1Id));
        }

        [Fact]
        public async Task DeleteByPlayerIdAsync_DoesNotRemoveOtherPlayersGames()
        {
            var player1 = DomainPlayer(Player1Id, "Alice");
            var player2 = DomainPlayer(Player2Id, "Bob");

            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player1, Choice.Rock, Choice.Paper));
            await _sut.CreateAsync(Game.Create(Guid.NewGuid(), player2, Choice.Scissors, Choice.Rock));

            await _sut.DeleteByPlayerIdAsync(Player1Id, CancellationToken.None);

            Assert.Single(_context.Games.Where(g => g.PlayerId == Player2Id));
        }

        public void Dispose() => _context.Dispose();
    }
}
