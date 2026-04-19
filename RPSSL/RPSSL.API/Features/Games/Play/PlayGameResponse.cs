namespace RPSSL.API.Features.Games.Play
{
    public class PlayGameResponse
    {
        public string Results { get; set; } = string.Empty;
        public int Player { get; set; }
        public int Computer { get; set; }
    }
}
