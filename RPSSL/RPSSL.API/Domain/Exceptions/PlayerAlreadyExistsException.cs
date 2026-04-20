namespace RPSSL.API.Domain.Exceptions
{
    /// <summary>Thrown when attempting to create a player whose name is already taken.</summary>
    public class PlayerAlreadyExistsException : DomainException
    {
        public PlayerAlreadyExistsException(string name)
            : base($"Player with name '{name}' already exists.") { }
    }
}
