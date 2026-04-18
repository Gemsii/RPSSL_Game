namespace RPSSL.API.Infrastructure.External.Options
{
    public class CodeChallengeApiOptions
    {
        public const string SectionName = "CodeChallengeApi";

        public string BaseAddress { get; set; } = string.Empty;
        public int RetryCount { get; set; } = 3;
    }
}
