namespace RPSSL.API.Domain.Interfaces
{
    public interface IRandomNumberService
    {
        Task<int> GetRandomNumberAsync();
    }
}
