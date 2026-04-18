namespace RPSSL.API.Infrastructure.Persistence.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
