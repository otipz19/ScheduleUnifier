using NPOI.HWPF.UserModel;
using NPOI.HWPF;
using NPOI.XWPF.UserModel;

namespace ConsoleApp.Parsing.Converters
{
    internal class DocToDocxConverter : IDocToDocxConverter
    {
        public string Convert(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open);
            HWPFDocument doc = new HWPFDocument(fs);
            XWPFDocument docx = new XWPFDocument();

            var docRange = doc.GetRange();

            for (int i = 0; i < docRange.NumParagraphs; i++)
            {
                XWPFParagraph targetParagraph = docx.CreateParagraph();
                var sourceParagraph = docRange.GetParagraph(i);
                for (int j = 0; j < sourceParagraph.NumCharacterRuns; j++)
                {
                    var sourceRun = sourceParagraph.GetCharacterRun(j);
                    XWPFRun targetRun = targetParagraph.CreateRun();
                    targetRun.SetText(sourceRun.Text);
                    //targetParagraph.AddRun(targetRun);
                }
            }

            var tableIterator = new TableIterator(docRange);
            while (tableIterator.HasNext())
            {
                var sourceTable = tableIterator.Next();
                XWPFTable targetTable = docx.CreateTable();
                for (int i = 0; i < sourceTable.NumRows; i++)
                {
                    XWPFTableRow targetRow = targetTable.CreateRow();
                    var sourceRow = sourceTable.GetRow(i);
                    for (int j = 0; j < sourceRow.NumCells(); j++)
                    {
                        var sourceCell = sourceRow.GetCell(j);
                        var targetCell = targetRow.CreateCell();
                        targetCell.SetText(sourceCell.Text);
                    }
                }
            }

            string convertedFilePath = filePath.Replace(".doc", ".docx");
            using (FileStream stream = new FileStream(convertedFilePath, FileMode.Create, FileAccess.Write))
            {
                docx.Write(stream);
            }
            return convertedFilePath;
        }
    }
}
