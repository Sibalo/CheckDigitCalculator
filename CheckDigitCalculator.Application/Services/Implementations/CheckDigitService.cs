using CheckDigitCalculator.App.Services.Interfaces;
using System.Collections.Generic;

namespace CheckDigitCalculator.App.Services.Implementations
{
    public class CheckDigitService : ICheckDigitService
    {
        public int CalculateCheckDigit(List<int> serialNumber)
        {
            if (serialNumber.Count == 1) return serialNumber[0];
            return CalculateCheckDigit(GenerateNextRow(serialNumber));
        }

        private List<int> GenerateNextRow(List<int> digits)
        {
            var result = new List<int>(digits.Count - 1);

            for (int i = 0; i < digits.Count - 1; i++)
            {
                int sum = digits[i] + digits[i + 1];
                result.Add(sum % 10);
            }
            return result;
        }
    }
}
