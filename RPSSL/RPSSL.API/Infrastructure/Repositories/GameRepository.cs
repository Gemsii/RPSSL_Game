using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.Mappers;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using DomainEntities = RPSSL.API.Domain.Entities;

namespace RPSSL.API.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly RpsslDbContext _context;

        public GameRepository(RpsslDbContext context)
        {
            _context = context;
        }

        public async Task<DomainEntities.Game> CreateAsync(DomainEntities.Game game)
        {
            var persistence = GameMapper.ToPersistence(game);
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.InsertGame";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = persistence.Id;
            command.Parameters.Add(idParam);

            var playerChoiceParam = command.CreateParameter();
            playerChoiceParam.ParameterName = "@PlayerChoice";
            playerChoiceParam.Value = (int)persistence.PlayerChoice;
            command.Parameters.Add(playerChoiceParam);

            var computerChoiceParam = command.CreateParameter();
            computerChoiceParam.ParameterName = "@ComputerChoice";
            computerChoiceParam.Value = (int)persistence.ComputerChoice;
            command.Parameters.Add(computerChoiceParam);

            var resultParam = command.CreateParameter();
            resultParam.ParameterName = "@Result";
            resultParam.Value = (int)persistence.Result;
            command.Parameters.Add(resultParam);

            var createdAtParam = command.CreateParameter();
            createdAtParam.ParameterName = "@CreatedAt";
            createdAtParam.Value = persistence.CreatedAt;
            command.Parameters.Add(createdAtParam);

            var playerIdParam = command.CreateParameter();
            playerIdParam.ParameterName = "@PlayerId";
            playerIdParam.Value = persistence.PlayerId;
            command.Parameters.Add(playerIdParam);

            await command.ExecuteNonQueryAsync();

            return game;
        }

        public async Task<IEnumerable<DomainEntities.Game>> GetByPlayerIdAsync(Guid playerId, int page, int pageSize)
        {
            // Call stored procedure via ADO.NET to avoid EF Core non-composable SQL issues
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.GetGamesByPlayerId";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var p1 = command.CreateParameter();
            p1.ParameterName = "@PlayerId";
            p1.Value = playerId;
            command.Parameters.Add(p1);

            var p2 = command.CreateParameter();
            p2.ParameterName = "@Page";
            p2.Value = page;
            command.Parameters.Add(p2);

            var p3 = command.CreateParameter();
            p3.ParameterName = "@PageSize";
            p3.Value = pageSize;
            command.Parameters.Add(p3);

            var persistenceGames = new List<Infrastructure.Persistence.Entities.Game>();

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var g = new Infrastructure.Persistence.Entities.Game
                    {
                        Id = reader.GetFieldValue<Guid>(reader.GetOrdinal("Id")),
                        PlayerChoice = reader.GetFieldValue<int>(reader.GetOrdinal("PlayerChoice")),
                        ComputerChoice = reader.GetFieldValue<int>(reader.GetOrdinal("ComputerChoice")),
                        Result = reader.GetFieldValue<int>(reader.GetOrdinal("Result")),
                        CreatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt")),
                        PlayerId = reader.GetFieldValue<Guid>(reader.GetOrdinal("PlayerId"))
                    };

                    persistenceGames.Add(g);
                }
            }

            if (!persistenceGames.Any())
                return Enumerable.Empty<DomainEntities.Game>();

            var playerIds = persistenceGames.Select(g => g.PlayerId).Distinct().ToList();
            var players = await _context.Players
                .Where(p => playerIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var g in persistenceGames)
            {
                if (players.TryGetValue(g.PlayerId, out var player))
                {
                    g.Player = player;
                }
            }

            return persistenceGames.Select(GameMapper.ToDomain);
        }

        public async Task DeleteByPlayerIdAsync(Guid playerId, CancellationToken ct)
        {
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync(ct);

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.DeleteGamesByPlayerId";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var p = command.CreateParameter();
            p.ParameterName = "@PlayerId";
            p.Value = playerId;
            command.Parameters.Add(p);

            await command.ExecuteNonQueryAsync(ct);
        }
    }
}
