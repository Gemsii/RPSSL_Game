using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IChoiceService
    {
        /// <summary>Maps a positive random number (1-100) to one of the five available choices.</summary>
        Choice GetByRandomNumber(PositiveNumber positiveNumber);
    }
}


