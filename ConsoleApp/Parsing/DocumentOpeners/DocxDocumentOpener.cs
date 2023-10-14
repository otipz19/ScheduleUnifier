using ScheduleUnifier.Parsing.TableModels;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    internal class DocxDocumentOpener : BaseDocumentOpener, IDocumentOpener
    {
        private WordprocessingDocument doc = default!;

        public DocxDocumentOpener(string filePath) : base(filePath) { }

        public IEnumerable<ITable> Tables { get; private set; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; private set; }

        protected override void OpenDocument(string filePath)
        {
            doc = WordprocessingDocument.Open(filePath, false);
            Tables = doc.MainDocumentPart!.Document.Body!.Elements<Table>().Select(t => new DocxTable(t)).ToArray();
            FacultyAndSpecializationParser = new DocxFacultyAndSpecializationParser(doc);
        }

        public void Close()
        {
            doc?.Dispose();
        }
    }
}
