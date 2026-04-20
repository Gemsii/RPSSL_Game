namespace RPSSL.API.Domain.Interfaces
{
    public interface IRandomNumberService
    {
        /// <summary>
        /// Fetches a random number from the external API.
        /// Retries with exponential backoff on failure.
        /// </summary>
        Task<int> GetRandomNumberAsync();
    }
}
