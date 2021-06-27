using SummaryApp.Models;
using System;
using System.IO;
using System.Text;

namespace SummaryApp
{
    public class SummaryProcessor
    {
        public static void SummarizeText(WordList inFileSentences, int summarizationFactor, WordList stopWords = null)
        {
            ProcessorUtils.PrintMessage("Starting summarization.", ConsoleColor.Green);

            // Get inFile data
            var inFileWords = inFileSentences.SelectMany(ProcessorUtils.GetWordsFromString);
            var inFileWordLength = inFileWords.Count();

            if (inFileWordLength > 0)
            {
                // Build frequency list (Sort by highest frequency)
                var inFileFreqList = ProcessorUtils.BuildWordFrequencyList(inFileWords);
                inFileFreqList.Sort();

                // Filter word frequency list
                inFileFreqList = stopWords != null ? FilterFrequencyList(inFileFreqList, stopWords) : inFileFreqList;

                var summarizedText = new StringBuilder();

                // Calculate summarization factor
                var currentSF = CalculateSummarizationFactor(GetWordCount(summarizedText), inFileWordLength);
                while (currentSF < summarizationFactor && inFileFreqList.Count() > 0 && inFileSentences.Count() > 0)
                {
                    var topWord = inFileFreqList.Head.Data;

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
            else
            {
                Console.WriteLine("Cannot summarize input, infile is empty.");
            }

        }

        private static string GetOutFileName()
        {
            return $"outFile-{DateTime.Now.Ticks}";
        }

        private static int GetWordCount(StringBuilder text)
        {
            return ProcessorUtils.GetWordsFromString(text.ToString()).Count();
        }

        private static WordDataList FilterFrequencyList(WordDataList frequencyList, WordList filterList)
        {
            var resultList = new WordDataList();

            var curr = frequencyList.Head;
            while (curr != null)
            {
                if (filterList.FirstOrDefault(i => i == curr.Data.Word) == null)
                {
                    resultList.Add(curr.Data);
                }
                curr = curr.Next;
            }

            return resultList;
        }

        private static int CalculateSummarizationFactor(int summarizeLength, int inputLength)
        {
            return (summarizeLength * 100) / inputLength;
        }

    }
}