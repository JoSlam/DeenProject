using SummaryApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SummaryApp
{
    public class SummaryProcessor
    {
        private string InFileLocation { get; set; }
        private string StopWordFileLocation { get; set; }
        public int SummarizationFactor { get; }
        private StringBuilder SummarizedText { get; set; }
        private int SummarizedWordCount
        {
            get
            {
                string text = SummarizedText?.ToString();
                return !string.IsNullOrEmpty(text) ? text.Split(" ").Count() : 0;
            }
        }

        public SummaryProcessor(string inFileLocation, string stopFileLocation, int sf)
        {
            this.InFileLocation = inFileLocation;
            this.StopWordFileLocation = stopFileLocation;
            this.SummarizationFactor = sf;
        }

        public void SummarizeText()
        {
            Console.WriteLine("Starting summarization.");

            // Get inFile data
            var inFileSentences = GetSentencesFromFile(InFileLocation);
            var inFileWords = inFileSentences.SelectMany(GetWordsFromString).ToList();
            var inFileWordLength = inFileWords.Count();

            
            // Build frequency list (Sort by highest frequency)
            var inFileFreqList = BuildWordFrequencyList(inFileWords);
            inFileFreqList.Sort(new WordDataComparer(SortOrder.Descending));

            // Load stopwords
            var stopWords = GetAllWordsFromFile(StopWordFileLocation);

            // Filter word frequency list
            stopWords.ForEach(i =>
            {
                WordData foundWord = null;
                if ((foundWord = inFileFreqList.FirstOrDefault(j => j.Word == i)) != null)
                {
                    inFileFreqList.Remove(foundWord);
                }
            });

            // Calculate summarization factor
            while (CalculateSummarizationFactor(SummarizedWordCount, inFileWordLength) < SummarizationFactor && inFileFreqList.Any())
            {
                var topWord = inFileFreqList.FirstOrDefault();

                // Find sentence with highest word frequency
                var sentenceWordOccurences = inFileSentences.Select(i =>
                {
                    var sentenceWordList = GetWordsFromString(i);
                    var wordOccurences = sentenceWordList.Count(i => i == topWord.Word);
                    return new WordData { Word = i, Frequency = wordOccurences };
                });

                var highestFrequency = sentenceWordOccurences.Max(i => i.Frequency);
                var sentenceWithMost = sentenceWordOccurences.FirstOrDefault(i => i.Frequency == highestFrequency);

                var newSummaryWordCount = SummarizedWordCount + GetWordsFromString(sentenceWithMost.Word).Count();
                var newSF = CalculateSummarizationFactor(newSummaryWordCount, inFileWordLength);

                if (newSF <= SummarizationFactor)
                {
                    SummarizedText.Append(sentenceWithMost.Word);
                    inFileSentences.Remove(sentenceWithMost.Word);
                    inFileFreqList.Remove(topWord);
                }
            }
        }

        private int CalculateSummarizationFactor(int summarizeLength, int inputLength)
        {
            return (summarizeLength * 100) / inputLength;
        }


        private void PrintWordList(List<WordData> wordList)
        {
            wordList.ForEach(i => Console.WriteLine($"Word: {i.Word} Frequency: {i.Frequency}"));
        }


        private List<WordData> BuildWordFrequencyList(List<string> wordList)
        {
            var wordDictionary = new Dictionary<string, WordData>();

            wordList.ForEach(word =>
            {
                if (wordDictionary.TryGetValue(word, out WordData existingNode))
                {
                    // Increment word count if node already exists
                    existingNode.Frequency++;
                }
                else
                {
                    wordDictionary.Add(word, new WordData() { Word = word, Frequency = 1 });
                }
            });

            return wordDictionary.Select(kvp => kvp.Value).ToList();
        }

        private List<string> GetAllWordsFromFile(string filePath)
        {
            return GetSentencesFromFile(filePath)
                .SelectMany(GetWordsFromString)
                .ToList();
        }

        private List<string> GetSentencesFromFile(string filePath)
        {
            // TODO: Assumption is that each new line contains a new sentence
            return File.ReadLines(filePath).ToList();
        }

        private List<string> GetWordsFromString(string sentence)
        {
            return sentence
                .Split(' ')
                .Where(i => !string.IsNullOrEmpty(i))
                .Select(CleanWord)
                .ToList();
        }

        private string CleanWord(string word)
        {
            return Regex.Replace(word, "[^0-9a-zA-Z]+", "");
        }

    }
}