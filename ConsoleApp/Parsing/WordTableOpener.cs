using ConsoleApp.Parsing.TableModels;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;

namespace ConsoleApp.Parsing
{
    internal class WordTableOpener : BaseTableOpener, ITableOpener
    {
        private WordprocessingDocument doc = default!;

        protected override ITable OpenTable(string filePath)
        {
            doc = WordprocessingDocument.Open(filePath, false);
            Table table = doc.MainDocumentPart!.Document.Body!.Elements<Table>().First();
            return new WordTable(table);
        }

        public void Close()
        {
            doc?.Dispose();
        }
    }
}
