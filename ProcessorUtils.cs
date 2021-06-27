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


        public static WordDataList BuildWordFrequencyList(WordList wordList)
        {
            var wordDictionary = new Dictionary<string, WordData>();

            var curr = wordList.Head;

            while (curr != null)
            {
                var word = curr.Data;
                if (wordDictionary.TryGetValue(word, out WordData existingNode))
                {
                    // Increment word count if node already exists
                    existingNode.Frequency++;
                }
                else
                {
                    wordDictionary.Add(word, new WordData() { Word = word, Frequency = 1 });
                }

                curr = curr.Next;
            }

            var wordDataList = wordDictionary.Select(kvp => kvp.Value).ToList();
            return new WordDataList(wordDataList);
        }

        public static WordList GetAllWordsFromInfile(string filePath)
        {
            return GetSentencesFromFile(filePath).SelectMany(GetWordsFromString);
        }

        public static WordList GetSentencesFromFile(string filePath)
        {
            // Read all file input
            string fileData = File.ReadAllText(filePath);
            string sentencePattern = @"\S(.+?[.!?])(?=\s+|$)";

            var sentences = new List<string>();
            // Parse based on a typical sentence structure delimited by ".!?"
            foreach (Match match in Regex.Matches(fileData, sentencePattern))
            {
                sentences.Add(match.Value);
            }
            return new WordList(sentences);
        }

        public static WordList GetWordsFromFile(string filePath)
        {
            var words = File.ReadLines(filePath).Select(i => i.Trim()).ToList();
            return new WordList(words);
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
