using SummaryApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SummaryApp
{
    public class SummaryProcessor
    {
        public static void SummarizeText(List<string> inFileSentences, int summarizationFactor, List<string> stopWords = null)
        {
           ProcessorUtils.PrintMessage("Starting summarization.", ConsoleColor.Green);

            // Get inFile data
            var inFileWords = inFileSentences.SelectMany(ProcessorUtils.GetWordsFromString).ToList();
            var inFileWordLength = inFileWords.Count();


            // Build frequency list (Sort by highest frequency)
            var inFileFreqList = ProcessorUtils.BuildWordFrequencyList(inFileWords);
            inFileFreqList.Sort(new WordDataComparer(SortOrder.Descending));

            // Filter word frequency list
            inFileFreqList = stopWords != null ? FilterFrequencyList(inFileFreqList, stopWords) : inFileFreqList;

            var summarizedText = new StringBuilder();

            // Calculate summarization factor
            var currentSF = CalculateSummarizationFactor(GetWordCount(summarizedText), inFileWordLength);
            while (currentSF < summarizationFactor && inFileFreqList.Any() && inFileSentences.Any())
            {
                var topWord = inFileFreqList.FirstOrDefault();

                // Find sentence with highest word frequency
                var sentenceWordOccurences = inFileSentences.Select(i =>
                {
                    var sentenceWordList = ProcessorUtils.GetWordsFromString(i);
                    var wordOccurences = sentenceWordList.Count(i => i == topWord.Word);
                    return new WordData { Word = i, Frequency = wordOccurences };
                });

                var highestFrequency = sentenceWordOccurences.Max(i => i.Frequency);
                var sentenceWithMost = sentenceWordOccurences.FirstOrDefault(i => i.Frequency == highestFrequency);

                var newSummaryWordCount = GetWordCount(summarizedText) + ProcessorUtils.GetWordsFromString(sentenceWithMost.Word).Count();
                var newSF = CalculateSummarizationFactor(newSummaryWordCount, inFileWordLength);

                // TODO: Change summarization factor to float / double
                if (newSF <= summarizationFactor)
                {
                    summarizedText.Append($"{sentenceWithMost.Word.Trim()} ");
                    currentSF = newSF;
                }

                inFileSentences.Remove(sentenceWithMost.Word);
                inFileFreqList.Remove(topWord);
            }

            // Print summarized text to console
            ProcessorUtils.PrintMessage("Summarized text", ConsoleColor.Green);
            Console.Write(summarizedText.ToString());

            // Print actual summarization factor
            ProcessorUtils.PrintMessage($"Actual summarization factor: {currentSF}", ConsoleColor.Green);

            // Print to file
            string outFileName = GetOutFileName();
            File.WriteAllText($"../../../Output/{outFileName}.txt", summarizedText.ToString());
        }

        private static string GetOutFileName()
        {
            return $"outFile-{DateTime.Now.Ticks}";
        }

        private static int GetWordCount(StringBuilder text)
        {
            return ProcessorUtils.GetWordsFromString(text.ToString()).Count();
        }

        private static List<WordData> FilterFrequencyList(List<WordData> frequencyList, List<string> filterList)
        {
            return frequencyList.Where(i => !filterList.Contains(i.Word)).ToList();
        }

        private static int CalculateSummarizationFactor(int summarizeLength, int inputLength)
        {
            return (summarizeLength * 100) / inputLength;
        }

    }
}