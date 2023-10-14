using ScheduleUnifier.FileProviders;
using ScheduleUnifier.Interpreting;
using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Parsing.DocumentParsers;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableOpeners;
using ScheduleUnifier.Serialization;

namespace ScheduleUnifier
{
    internal static class Program
    {
        private static string FullOutputDirectoryPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output");
        private static string FullOutputFilePath => Path.Join(FullOutputDirectoryPath, "output.json");

        private static readonly ITableInterpreter interpreter = new TableInterpreter();
        private static readonly ScheduleHandler scheduleHandler = new ScheduleHandler();

        static void Main(string[] args)
        {
            try
            {
                IFileProvider fileProvider = new HttpFileProvider(new string[]
                    { "https://my.ukma.edu.ua/files/schedule/2023/1/1472/3.xlsx" });
                //IFileProvider fileProvider = new LocalFileProvider();
                ProcessFiles(fileProvider.GetFilesToParse());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ProcessFiles((string filePath, bool isExcel)[] filesToParse)
        {
            foreach (var file in filesToParse)
            {
                ProcessDocumentInFile(file);
            }

            scheduleHandler.Serialize(FullOutputDirectoryPath, FullOutputFilePath);
        }

        private static void ProcessDocumentInFile((string filePath, bool isExcel) file)
        {
            ParsedDocument parsedDocument = ParseDocument(file);

            IEnumerable<RecordModel> records = interpreter.Interpret(parsedDocument);

            foreach (var record in records)
            {
                scheduleHandler.Schedule.Add(record);
            }
        }

        private static ParsedDocument ParseDocument((string filePath, bool isExcel) file)
        {
            IDocumentOpener documentOpener = ResolveDocumentOpenerType(file);
            IDocumentParser documentParser = new DocumentParser(documentOpener);
            return documentParser.Parse();
        }

        private static IDocumentOpener ResolveDocumentOpenerType((string filePath, bool isExcel) file)
        {
            return file.isExcel ? new ExcelDocumentOpener(file.filePath) : new DocxDocumentOpener(file.filePath);
        }
    }
}