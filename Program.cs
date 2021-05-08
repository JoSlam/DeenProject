using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartupAsync();
        }

        static async Task StartupAsync()
        {
            // stop file data
            var stopFileLocation = "../../../Data/stopwords.txt";
            List<string> stopWords = ProcessorUtils.GetAllWordsFromFile(stopFileLocation);

            // inFile data
            var inFileLocation = "";

            DisplayMenu();
            ConsoleKeyInfo input = Console.ReadKey();

            while (input.Key != ConsoleKey.NumPad6 || input.Key != ConsoleKey.D6)
            {
                switch (input.Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        if (stopWords != null && stopWords.Count > 0)
                        {
                            ProcessorUtils.PrintMessage("Printing stopword list:", ConsoleColor.Green);
                            stopWords.ForEach(Console.WriteLine);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("Stopword list is empty.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        Console.WriteLine(Environment.NewLine);
                        Console.Write("Enter infile location: ");
                        inFileLocation = Console.ReadLine();

                        if (!File.Exists(inFileLocation))
                        {
                            ProcessorUtils.PrintMessage("File does not exist, enter a valid file location.", ConsoleColor.Red);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("File loaded.", ConsoleColor.Green);
                        }
                        break;

                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                        if (!string.IsNullOrEmpty(inFileLocation))
                        {
                            var sentences = ProcessorUtils.GetSentencesFromFile(inFileLocation);
                            var inFileWords = sentences.SelectMany(ProcessorUtils.GetWordsFromString).ToList();
                            var inFileFreqList = ProcessorUtils.BuildWordFrequencyList(inFileWords);
                            inFileFreqList.Sort(new WordDataComparer(SortOrder.Descending));

                            ProcessorUtils.PrintMessage("Printing word frequency list:", ConsoleColor.Green);
                            ProcessorUtils.PrintWordList(inFileFreqList);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile entered.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                        if (!string.IsNullOrEmpty(inFileLocation))
                        {
                            var sentences = ProcessorUtils.GetSentencesFromFile(inFileLocation);
                            ProcessorUtils.PrintMessage("Printing input sentences.", ConsoleColor.Green);

                            sentences.ForEach(Console.WriteLine);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile entered.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.NumPad5:
                    case ConsoleKey.D5:
                        if (!string.IsNullOrEmpty(inFileLocation))
                        {
                            int summarizationFactor;

                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine("Enter summarization factor (1-100%):");
                            while (int.TryParse(Console.ReadLine(), out summarizationFactor)
                                && (summarizationFactor <= 0 || summarizationFactor > 100))
                            {
                                Console.Write("Please enter a valid number (1-100): ");
                            }

                            var inFileSentences = ProcessorUtils.GetSentencesFromFile(inFileLocation);
                            SummaryProcessor.SummarizeText(inFileSentences, summarizationFactor, stopWords);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile entered.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return;
                    default:
                        ProcessorUtils.PrintMessage("Unknown input.", ConsoleColor.Red);
                        break;
                }

                DisplayMenu();
                input = Console.ReadKey();
            }

            Console.WriteLine(Environment.NewLine);
            await StartupAsync();
        }

        static void DisplayMenu()
        {
            ProcessorUtils.PrintMessage("Select an option:", ConsoleColor.Green);
            Console.WriteLine("1. Display stop words.");
            Console.WriteLine("2. Enter infile location.");
            Console.WriteLine("3. Display word frequency distribution.");
            Console.WriteLine("4. Display infile sentences.");
            Console.WriteLine("5. Summarize text.");
            Console.WriteLine("6. Quit.");
            Console.Write("Option #: ");
        }

    }
}
