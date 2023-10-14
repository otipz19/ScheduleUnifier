using ConsoleApp.Parsing.TableModels;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using ConsoleApp.Parsing.FacultyAndSpecializationParsers;

namespace ConsoleApp.Parsing.TableOpeners
{
    internal class DocxTableOpener : BaseTableOpener, ITableOpener
    {
        private WordprocessingDocument doc = default!;

        public DocxTableOpener(string filePath) : base(filePath) { }

        public ITable Table { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            doc = WordprocessingDocument.Open(filePath, false);
            Table = new DocxTable(doc.MainDocumentPart!.Document.Body!.Elements<Table>().First());
            FacultyAndSpecializationParser = new DocxFacultyAndSpecializationParser(doc);
        }

        public void Close()
        {
            doc?.Dispose();
        }
    }
}
