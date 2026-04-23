using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Infrastructure.Mappers;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using DomainEntities = RPSSL.API.Domain.Entities;

namespace RPSSL.API.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly RpsslDbContext _context;

        public PlayerRepository(RpsslDbContext context)
        {
            _context = context;
        }

        public async Task<DomainEntities.Player?> GetByIdAsync(Guid id)
        {
            // Use ADO.NET to call stored procedure to avoid EF Core composition issues
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.GetPlayerById";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = id;
            command.Parameters.Add(idParam);

            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.Read())
                return null;

            var entity = new Infrastructure.Persistence.Entities.Player
            {
                Id = reader.GetFieldValue<Guid>(reader.GetOrdinal("Id")),
                Name = reader.GetFieldValue<string>(reader.GetOrdinal("Name"))
            };

            return PlayerMapper.ToDomain(entity);
        }

        public async Task<DomainEntities.Player?> GetByNameAsync(string name)
        {
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.GetPlayerByName";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@Name";
            nameParam.Value = name;
            command.Parameters.Add(nameParam);

            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.Read())
                return null;

            var entity = new Infrastructure.Persistence.Entities.Player
            {
                Id = reader.GetFieldValue<Guid>(reader.GetOrdinal("Id")),
                Name = reader.GetFieldValue<string>(reader.GetOrdinal("Name"))
            };

            return PlayerMapper.ToDomain(entity);
        }

        public async Task<DomainEntities.Player> CreateAsync(DomainEntities.Player player)
        {
            var persistence = PlayerMapper.ToPersistence(player);
            var connString = _context.Database.GetDbConnection().ConnectionString;
            await using var connection = new SqlConnection(connString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.InsertPlayer";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = persistence.Id;
            command.Parameters.Add(idParam);

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@Name";
            nameParam.Value = persistence.Name ?? (object)DBNull.Value;
            command.Parameters.Add(nameParam);

            await command.ExecuteNonQueryAsync();

            return player;
        }
    }
}

