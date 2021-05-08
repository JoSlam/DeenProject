using System;
using System.IO;

namespace SummaryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inFileLocation = "";
            var stopFileLocation = "../../../Data/stopwords.txt";
            int summarizationFactor;

            Console.WriteLine("Enter infile location:");
            inFileLocation = Console.ReadLine();

            while (!File.Exists(inFileLocation))
            {
                Console.WriteLine("Please enter a valid file location.");
                inFileLocation = Console.ReadLine();
            }

            Console.WriteLine("Enter summarization factor (1-100%):");
            while (int.TryParse(Console.ReadLine(), out summarizationFactor) && summarizationFactor <= 0)
            {
                Console.WriteLine("Please enter a valid number (1-100).");
            }

            var app = new SummaryProcessor(inFileLocation, stopFileLocation, summarizationFactor);
            app.SummarizeText();
        }
    }
}
