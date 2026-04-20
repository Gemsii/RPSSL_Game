namespace RPSSL.API.Features.Games.GetScoreboard
{
    public class ScoreboardEntryResponse
    {
        public string Results { get; set; } = string.Empty;
        public int PlayerChoice { get; set; }
        public int ComputerChoice { get; set; }
    }
}
