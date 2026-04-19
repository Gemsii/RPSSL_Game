using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.ValueObjects;
using DomainEntities = RPSSL.API.Domain.Entities;
using PersistenceEntities = RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.API.Infrastructure.Mappers
{
    public static class GameMapper
    {
        public static PersistenceEntities.Game ToPersistence(DomainEntities.Game domain)
        {
            return new PersistenceEntities.Game
            {
                Id = domain.Id,
                PlayerId = domain.Player.Id,
                PlayerChoice = (int)domain.PlayerChoice,
                ComputerChoice = (int)domain.ComputerChoice,
                Result = (int)domain.Result,
                CreatedAt = domain.CreatedAt
            };
        }

        public static DomainEntities.Game ToDomain(PersistenceEntities.Game persistence)
        {
            var playerName = PlayerName.Create(persistence.Player.Name);
            var player = DomainEntities.Player.Create(persistence.Player.Id, playerName);

            return DomainEntities.Game.Create(
                persistence.Id,
                player,
                (Choice)persistence.PlayerChoice,
                (Choice)persistence.ComputerChoice
            );
        }
    }
}
