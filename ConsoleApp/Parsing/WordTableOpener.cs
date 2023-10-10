using ConsoleApp.Parsing.TableModels;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace ConsoleApp.Parsing
{
    internal class WordTableOpener : BaseTableOpener, ITableOpener
    {
        private WordprocessingDocument doc = default!;

        public WordTableOpener(string filePath) : base(filePath) { }

        public ITable Table { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            doc = WordprocessingDocument.Open(filePath, false);
            Table = new WordTable(doc.MainDocumentPart!.Document.Body!.Elements<Table>().First());
            FacultyAndSpecializationParser = new WordFacultyAndSpecializationParser(doc);
        }

        public void Close()
        {
            doc?.Dispose();
        }
    }
}
