namespace RPSSL.API.Domain.Exceptions
{
    /// <summary>Thrown when a player with the given identifier does not exist.</summary>
    public class PlayerNotFoundException : DomainException
    {
        public PlayerNotFoundException(Guid playerId)
            : base($"Player with id '{playerId}' was not found.") { }
    }
}
