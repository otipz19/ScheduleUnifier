using ScheduleUnifier.FileProviders;
using ScheduleUnifier.Interpreting;
using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Parsing.DocumentParsers;
using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableOpeners;
using ScheduleUnifier.Serialization;

namespace ScheduleUnifier
{
    public class Unifier
    {
        private readonly ITableInterpreter interpreter = new TableInterpreter();
        private readonly ScheduleHandler scheduleHandler = new ScheduleHandler();
        private readonly IFileProvider fileProvider;

        private static string DefaultInputPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "input");
        private static string DefaultOutputDirectoryPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output");
        private static string DefaultOutputFilePath => Path.Join(DefaultOutputDirectoryPath, "output.json");
        private static string[] DefaultUrls => new string[]
        {
            "https://my.ukma.edu.ua/files/schedule/2023/1/1475/1.docx",
            "https://my.ukma.edu.ua/files/schedule/2023/1/1472/3.xlsx",
            "https://my.ukma.edu.ua/files/schedule/2023/1/1483/3.xlsx",
            "https://my.ukma.edu.ua/files/schedule/2023/1/1562/1.docx",
        };

        private string inputDirPath = DefaultInputPath;
        private string outputDirPath = DefaultOutputDirectoryPath;
        private string outputFilePath = DefaultOutputFilePath;
        private string[] urls = DefaultUrls;

        public Unifier(
            string? inputDirPath = null,
            string? outputDirPath = null,
            bool isHttp = false,
            IEnumerable<string>? urls = null)
        {
            SetPath(ref this.inputDirPath, inputDirPath);
            SetPath(ref this.outputDirPath, outputDirPath);

            if(urls is not null && urls.Any())
            {
                this.urls = urls.ToArray();
            }

            fileProvider = isHttp ? new HttpFileProvider(this.urls) : new LocalFileProvider();
        }

        public void Run()
        {
            try
            {
                ProcessFiles(fileProvider.GetFilesToParse(inputDirPath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void SetPath(ref string config, string? input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (Path.IsPathRooted(input))
                {
                    config = input;
                }
                else
                {
                    config = Path.Join(AppDomain.CurrentDomain.BaseDirectory, input);
                }
            }
        }

        private void ProcessFiles((string filePath, bool isExcel)[] filesToParse)
        {
            foreach (var file in filesToParse)
            {
                try
                {
                    ProcessDocumentInFile(file);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception occured while processing file: {file.filePath}\n{ex.Message}\n{ex.StackTrace}");
                }
            }

            scheduleHandler.Serialize(outputDirPath, outputFilePath);
        }

        private void ProcessDocumentInFile((string filePath, bool isExcel) file)
        {
            ParsedDocument parsedDocument = ParseDocument(file);

            IEnumerable<RecordModel> records = interpreter.Interpret(parsedDocument);

            foreach (var record in records)
            {
                scheduleHandler.Schedule.Add(record);
            }
        }

        private ParsedDocument ParseDocument((string filePath, bool isExcel) file)
        {
            IDocumentOpener documentOpener = ResolveDocumentOpenerType(file);
            IDocumentParser documentParser = new DocumentParser(documentOpener);
            return documentParser.Parse();
        }

        private IDocumentOpener ResolveDocumentOpenerType((string filePath, bool isExcel) file)
        {
            return file.isExcel ? new ExcelDocumentOpener(file.filePath) : new DocxDocumentOpener(file.filePath);
        }
    }
}
