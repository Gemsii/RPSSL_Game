namespace RPSSL.API.Infrastructure.Persistence.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public int PlayerChoice { get; set; }
        public int ComputerChoice { get; set; }
        public int Result { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
