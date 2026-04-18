using RPSSL.API.Domain.Common;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.API.Domain.Entities
{
    public class Player : Entity
    {
        public PlayerName Name { get; private set; }

        private Player(Guid id, PlayerName name) : base(id)
        {
            Name = name;
        }

        public static Player Create(Guid id, PlayerName name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Player id must be provided");

            if (name == null)
                throw new ArgumentException("Player name must be provided");

            return new Player(id, name);
        }
    }
}
