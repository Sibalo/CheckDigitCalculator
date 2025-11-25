using System.Collections.Generic;

namespace CheckDigitCalculator.App.Services.Interfaces
{
    public interface ICheckDigitService
    {
        int CalculateCheckDigit(List<int> serialNumber);
    }
}
