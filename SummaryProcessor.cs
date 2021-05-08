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
            var inFileData = GetAllWordsFromFile(InFileLocation);
            var inFileWordLength = inFileData.Count();
            var inFileFreqList = BuildWordFrequencyList(inFileData);
            
            // Sort by highest frequency
            inFileFreqList.Sort(new WordDataComparer(SortOrder.Descending));

            // TODO: get inFile sentences
            // loop through sentences
            // Calculate summarization factor

            // Print list
            inFileFreqList.ForEach(i => Console.WriteLine($"Word: {i.Word} Frequency: {i.Frequency}"));
        }

        private int CalculateSummarizationFactor(int inFileLength)
        {
            return (SummarizedWordCount * 100) / inFileLength;
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