using ConsoleApp.Interpreting;
using ConsoleApp.Interpreting.Models;
using ConsoleApp.Parsing.Converters;
using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.Models;
using ConsoleApp.Parsing.TableOpeners;
using ConsoleApp.Parsing.TableParsers;
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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
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
            ITableParser tableParser = new TableParser(tableOpener);
            return tableParser.Parse();
        }

        private static ITableOpener ResolveTableOpenerType((string filePath, bool isExcel) file)
        {
            return file.isExcel ? new ExcelTableOpener(file.filePath) : new DocxTableOpener(file.filePath);
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
            var filesToParse = inputDir.GetFiles()
                .Where(f => parsableExtensions.Contains(f.Extension));

            if (!filesToParse.Any())
            {
                throw new InputFilesNotFoundException();
            }

            var docsToConvert = filesToParse.Where(f => f.Extension.Equals(".doc"));
            filesToParse.Where(f => !docsToConvert.Contains(f));
            var convertedDocs = new List<FileInfo>();

            foreach(var doc in docsToConvert)
            {
                convertedDocs.Add(ConvertDocToDocx(doc));
            }

            var result = new List<(string filePath, bool isExcel)>();
            result.AddRange(filesToParse.Select(f => (f.FullName, f.Extension.Equals(".xlsx"))));
            result.AddRange(convertedDocs.Select(f => (f.FullName, f.Extension.Equals(".xlsx"))));

            return result.ToArray();
        }

        private static FileInfo ConvertDocToDocx(FileInfo file)
        {
            var converter = new B2xConverter();
            string newFileName = converter.Convert(file.FullName);
            file.Delete();
            return new FileInfo(newFileName);
        }
    }
}