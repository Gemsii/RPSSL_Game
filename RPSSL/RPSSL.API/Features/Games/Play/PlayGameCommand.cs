namespace RPSSL.API.Features.Games.Play
{
    public class PlayGameCommand
    {
        public int Choice { get; set; }
        public Guid? PlayerId { get; set; }
    }
}
