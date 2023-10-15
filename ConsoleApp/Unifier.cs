using ScheduleUnifier.Configuration;
using ScheduleUnifier.Exceptions;
using ScheduleUnifier.FileProviders;
using ScheduleUnifier.Interpreting;
using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Parsing.DocumentParsers;
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

        private readonly ConfigurationModel config;

        public Unifier(ConfigurationModel config)
        {
            this.config = config;
            fileProvider = config.UseHttp ? new HttpFileProvider(config.Urls) : new LocalFileProvider();
        }

        public int FilesProcessed { get; private set; }
        public int FilesTotal { get; private set; }


        public void Run()
        {
            try
            {
                var filesToParse = fileProvider.GetFilesToParse(config.InputDirPath);
                FilesTotal = filesToParse.Length;
                ProcessFiles(filesToParse);
            }
            catch(UnsupportedDocFormatException ex)
            {
                Console.WriteLine(ex.Message);
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
                    FilesProcessed++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occured while processing file: {file.filePath}\n{ex.Message}\n");
                }
            }

            scheduleHandler.Serialize(config.OutputDirPath, Path.Join(config.OutputDirPath, "output.json"));
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
