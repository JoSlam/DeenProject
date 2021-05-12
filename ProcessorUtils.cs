using SummaryApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SummaryApp
{
    public class ProcessorUtils
    {

        public static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintWordList(List<WordData> wordList)
        {
            wordList.ForEach(i => Console.WriteLine($"Word: {i.Word} Frequency: {i.Frequency}"));
        }

        public static List<WordData> BuildWordFrequencyList(List<string> wordList)
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

        public static List<string> GetAllWordsFromFile(string filePath)
        {
            return GetSentencesFromFile(filePath)
                .SelectMany(GetWordsFromString)
                .ToList();
        }

        public static WordList GetSentencesFromFile(string filePath)
        {
            // It is assumed that each new line contains a new sentence
            var sentences = File.ReadLines(filePath).Select(i => i.Trim()).ToList();
            return new WordList(sentences);
        }

        public static WordList GetWordsFromString(string sentence)
        {
            var words = sentence
                .Split(' ')
                .Where(i => !string.IsNullOrEmpty(i))
                .Select(CleanWord)
                .ToList();

            var wordList = new WordList(words);

            return wordList;
        }

        public static string CleanWord(string word)
        {
            return Regex.Replace(word, "[^0-9a-zA-Z]+", "");
        }
    }
}
