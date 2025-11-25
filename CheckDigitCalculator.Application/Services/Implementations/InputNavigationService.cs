using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CheckDigitCalculator.Application.Services.Implementations
{
    public class InputNavigationService
    {
        public int GetCurrentIndex(TextBox[] boxes, TextBox currentBox)
        {
            return Array.IndexOf(boxes, currentBox);
        }

        public int? GetNextIndex(TextBox[] boxes, TextBox currentBox)
        {
            int index = GetCurrentIndex(boxes, currentBox);
            if (index < boxes.Length - 1)
                return index + 1;
            return null;
        }

        public int? GetPreviousIndex(TextBox[] boxes, TextBox currentBox)
        {
            int index = GetCurrentIndex(boxes, currentBox);
            if (index > 0)
                return index - 1;
            return null;
        }

        public bool AllFilled(TextBox[] boxes)
        {
            return boxes.All(b => !string.IsNullOrWhiteSpace(b.Text));
        }

        public List<int> ExtractValues(TextBox[] boxes)
        {
            return boxes
                .Where(b => int.TryParse(b.Text, out _))
                .Select(b => int.Parse(b.Text))
                .ToList();
        }
    }
}
