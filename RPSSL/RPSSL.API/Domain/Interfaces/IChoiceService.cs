using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.API.Domain.Interfaces
{
    public interface IChoiceService
    {
        Choice GetByRandomNumber(PositiveNumber positiveNumber);
    }
}


