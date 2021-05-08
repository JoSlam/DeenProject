using SummaryApp.Models;
using System.Collections.Generic;

namespace SummaryApp
{
    public class WordDataComparer : IComparer<WordData>
    {
        public SortOrder SortOrder { get; set; }
        public WordDataComparer(SortOrder sortOrder)
        {
            this.SortOrder = sortOrder;
        }

        public int Compare(WordData x, WordData y)
        {
            int result;
            if (x.Frequency > y.Frequency)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }

            if (SortOrder == SortOrder.Descending)
            {
                result *= -1;
            }
            return result;
        }
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
