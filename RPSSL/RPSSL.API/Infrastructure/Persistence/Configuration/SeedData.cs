namespace RPSSL.API.Infrastructure.Persistence.Configuration
{
    public static class SeedData
    {
        public static readonly Guid ComputerPlayerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const string ComputerPlayerName = "Computer";

        public static readonly Guid AnonymousPlayerId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public const string AnonymousPlayerName = "Anonymous";
    }
}
