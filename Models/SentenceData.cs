using System.Collections.Generic;

namespace SummaryApp.Models
{
    public class SentenceData
    {
        public string Sentence { get; set; }
        public Dictionary<string, int> WordFrequencyDistribution { get; set; }
    }
}
