using RPSSL.API.Domain.ValueObjects;
using DomainEntities = RPSSL.API.Domain.Entities;
using PersistenceEntities = RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.API.Infrastructure.Mappers
{
    public static class PlayerMapper
    {
        public static PersistenceEntities.Player ToPersistence(DomainEntities.Player domain)
        {
            return new PersistenceEntities.Player
            {
                Id = domain.Id,
                Name = domain.Name.Value
            };
        }

        public static DomainEntities.Player ToDomain(PersistenceEntities.Player persistence)
        {
            var playerName = PlayerName.Create(persistence.Name);
            return DomainEntities.Player.Create(persistence.Id, playerName);
        }
    }
}
