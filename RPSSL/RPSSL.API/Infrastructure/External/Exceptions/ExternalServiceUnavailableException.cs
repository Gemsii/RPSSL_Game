namespace RPSSL.API.Infrastructure.External.Exceptions
{
    /// <summary>Thrown when the external random number service is unavailable after all retry attempts.</summary>
    public class ExternalServiceUnavailableException : Exception
    {
        public ExternalServiceUnavailableException()
            : base("External random number service is unavailable after multiple retries.") { }
    }
}
