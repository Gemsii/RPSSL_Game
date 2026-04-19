using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.API.Services
{
    public class ChoiceService : IChoiceService
    {
        public Choice GetByRandomNumber(PositiveNumber positiveNumber)
        {
            if (positiveNumber is null)
                throw new ArgumentNullException(nameof(positiveNumber));

            var enumLength = Enum.GetValues(typeof(Choice)).Length;
            var choiceIndex = (positiveNumber.Value - 1) % enumLength + 1;

            return (Choice)choiceIndex;
        }
    }
}


