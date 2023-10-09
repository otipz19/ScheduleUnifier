using ConsoleApp.Interpreting;
using ConsoleApp.Interpreting.Models;
using ConsoleApp.Parsing;
using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.Models;
using ConsoleApp.Parsing.TableModels;
using ConsoleApp.Serialization.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ConsoleApp
{
    internal static class Program
    {
        private static string FullInputPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "input");
        private static string FullOutputDirectoryPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output");
        private static string FullOutputFilePath => Path.Join(FullOutputDirectoryPath, "output.json");

        private static readonly TableInterpreter interpreter = new TableInterpreter();
        private static readonly Schedule schedule = new Schedule();

        static void Main(string[] args)
        {
            try
            {
                var filesToParse = GetFilesToParse();
                
                foreach (var file in filesToParse)
                {
                    ProcessTableInFile(file);
                }

                SerializeSchedule();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ProcessTableInFile((string filePath, bool isExcel) file)
        {
            ParsedTable parsedTable = ParseTable(file);

            IEnumerable<RecordModel> records = interpreter.Interpret(parsedTable);

            foreach (var record in records)
            {
                schedule.Add(record);
            }
        }

        private static ParsedTable ParseTable((string filePath, bool isExcel) file)
        {
            ITableOpener tableOpener = ResolveTableOpenerType(file);
            ITable table = tableOpener.TryOpen(file.filePath);
            ITableParser tableParser = new TableParser(table);
            return tableParser.Parse();
        }

        private static ITableOpener ResolveTableOpenerType((string filePath, bool isExcel) file)
        {
            return file.isExcel ? new ExcelTableOpener() : new WordTableOpener();
        }

        private static void SerializeSchedule()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            string json = JsonSerializer.Serialize<Schedule>(schedule, options);

            if (!Directory.Exists(FullOutputDirectoryPath))
            {
                Directory.CreateDirectory(FullOutputDirectoryPath);
            }

            File.WriteAllText(FullOutputFilePath, json);
        }

        private static (string filePath, bool isExcel)[] GetFilesToParse()
        {
            var inputDir = new DirectoryInfo(FullInputPath);

            if (!inputDir.Exists)
            {
                throw new InputFilesNotFoundException();
            }

            string[] parsableExtensions = new string[] { ".doc", ".docx", ".xlsx" };
            var filesToParse =  inputDir.GetFiles()
                .Where(f => parsableExtensions.Contains(f.Extension))
                .Select(f => (f.FullName, f.Extension.Equals(".xlsx")))
                .ToArray();

            if (!filesToParse.Any())
            {
                throw new InputFilesNotFoundException();
            }

            return filesToParse;
        }
    }
}