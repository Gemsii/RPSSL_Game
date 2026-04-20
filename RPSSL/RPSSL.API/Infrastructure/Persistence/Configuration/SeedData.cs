using RPSSL.API.Domain.Constants;

namespace RPSSL.API.Infrastructure.Persistence.Configuration
{
    public static class SeedData
    {
        public static readonly Guid ComputerPlayerId = WellKnownPlayers.ComputerPlayerId;
        public const string ComputerPlayerName = "Computer";

        public static readonly Guid AnonymousPlayerId = WellKnownPlayers.AnonymousPlayerId;
        public const string AnonymousPlayerName = "Anonymous";
    }
}
