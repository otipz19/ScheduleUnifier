using ConsoleApp.Interpreting;
using ConsoleApp.Interpreting.Models;
using ConsoleApp.Parsing;
using ConsoleApp.Parsing.Models;
using ConsoleApp.Serialization.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new TableParser();
            var interpreter = new TableInterpreter();
            var schedule = new Schedule();

            Console.WriteLine("Input file names to parse:");
            string[] fileNames = Console.ReadLine()
                .Split(' ', StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
            if (!fileNames.Any())
            {
                fileNames = new string[] { "fen.xlsx", "ipz.xlsx" };
            }

            try
            {
                foreach (string fileName in fileNames)
                {
                    string filePath = GetFullPath(fileName);
                    ParsedTable parsedTable = parser.Parse(filePath);
                    IEnumerable<RecordModel> records = interpreter.Interpret(parsedTable);

                    foreach (var record in records)
                    {
                        schedule.Add(record);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            string json = JsonSerializer.Serialize<Schedule>(schedule, options);
            File.WriteAllText(GetFullPath("output.json"), json);
        }

        private static string GetFullPath(string fileName) => Path.Join(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }
}