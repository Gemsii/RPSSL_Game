namespace RPSSL.API.Features.Games.GetScoreboard
{
    public class GetScoreboardQuery
    {
        public Guid PlayerId { get; set; }
        public int Page { get; set; }
    }
}

