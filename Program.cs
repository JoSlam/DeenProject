using System;
using System.IO;
using System.Threading.Tasks;

namespace SummaryApp
{
    // -J
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartupAsync();
        }

        static async Task StartupAsync()
        {
            // File paths
            var stopFilePath = "";
            var inFilePath = "";

            DisplayMenu();
            ConsoleKeyInfo input = Console.ReadKey();

            while (input.Key != ConsoleKey.NumPad6 || input.Key != ConsoleKey.D6)
            {
                switch (input.Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        Console.WriteLine(Environment.NewLine);
                        Console.Write("Enter stopword file location: ");
                        stopFilePath = Console.ReadLine();

                        if (!File.Exists(stopFilePath))
                        {
                            ProcessorUtils.PrintMessage("File does not exist, enter a valid file location.", ConsoleColor.Red);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("Stopword file loaded.", ConsoleColor.Green);
                        }
                        break;

                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:

                        if (!string.IsNullOrEmpty(stopFilePath))
                        {
                            var stopWords = ProcessorUtils.GetWordsFromFile(stopFilePath);
                            if (stopWords.Count() > 0)
                            {
                                ProcessorUtils.PrintMessage("Printing stopword list:", ConsoleColor.Green);
                                stopWords.PrintWordList();
                            }
                            else
                            {
                                ProcessorUtils.PrintMessage("Stopword list is empty.", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No stop word file path set.", ConsoleColor.Red);
                        }

                        break;

                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                        Console.WriteLine(Environment.NewLine);
                        Console.Write("Enter infile location: ");
                        inFilePath = Console.ReadLine();

                        if (!File.Exists(inFilePath))
                        {
                            ProcessorUtils.PrintMessage("File does not exist, enter a valid file location.", ConsoleColor.Red);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("Infile loaded.", ConsoleColor.Green);
                        }
                        break;

                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                        if (!string.IsNullOrEmpty(inFilePath))
                        {
                            var sentences = ProcessorUtils.GetSentencesFromFile(inFilePath);
                            var inFileWords = sentences.SelectMany(ProcessorUtils.GetWordsFromString);
                            var inFileFreqList = ProcessorUtils.BuildWordFrequencyList(inFileWords);
                            inFileFreqList.Sort();


                            if (inFileFreqList.Count() > 0)
                            {
                                ProcessorUtils.PrintMessage("Printing word frequency list:", ConsoleColor.Green);
                                inFileFreqList.PrintWordDataList();
                            }
                            else
                            {
                                ProcessorUtils.PrintMessage("Frequency list is empty.", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile path set.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.NumPad5:
                    case ConsoleKey.D5:
                        if (!string.IsNullOrEmpty(inFilePath))
                        {
                            var sentences = ProcessorUtils.GetSentencesFromFile(inFilePath);
                            if (sentences.Count() > 0)
                            {
                                ProcessorUtils.PrintMessage("Printing input sentences.", ConsoleColor.Green);
                                sentences.PrintWordList();
                            }
                            else
                            {
                                ProcessorUtils.PrintMessage("Infile is empty.", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile entered.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.NumPad6:
                    case ConsoleKey.D6:
                        if (!string.IsNullOrEmpty(inFilePath))
                        {
                            int summarizationFactor;
                            var stopWords = ProcessorUtils.GetAllWordsFromInfile(stopFilePath);

                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine("Enter summarization factor (1-100%):");
                            while (int.TryParse(Console.ReadLine(), out summarizationFactor)
                                && (summarizationFactor <= 0 || summarizationFactor > 100))
                            {
                                Console.Write("Please enter a valid number (1-100): ");
                            }

                            var inFileSentences = ProcessorUtils.GetSentencesFromFile(inFilePath);
                            SummaryProcessor.SummarizeText(inFileSentences, summarizationFactor, stopWords);
                        }
                        else
                        {
                            ProcessorUtils.PrintMessage("No infile entered.", ConsoleColor.Red);
                        }
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
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
            Console.WriteLine("1. Enter stop word file location.");
            Console.WriteLine("2. Display stop words.");
            Console.WriteLine("3. Enter infile location.");
            Console.WriteLine("4. Display word frequency distribution.");
            Console.WriteLine("5. Display infile sentences.");
            Console.WriteLine("6. Summarize text.");
            Console.WriteLine("7. Quit.");
            Console.Write("Option #: ");
        }

    }
}
