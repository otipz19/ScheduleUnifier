using DocumentFormat.OpenXml.Drawing;

namespace ConsoleApp.Parsing.TableModels
{
    internal class WordTable : ITable
    {
        private readonly Table table;

        public WordTable(Table table)
        {
            this.table = table;
        }

        public string this[int row, int col]
        {
            get
            {
                TableCell cell = table.Elements<TableRow>().ElementAt(row)
                    .Elements<TableCell>().ElementAt(col);
                return GetCellText(cell);
            }
        }

        public int GetLastNotEmptyRow()
        {
            TableRow[] rows = table.Elements<TableRow>().ToArray();
            int lastNotEmptyRowIndex = rows.Length - 1;
            while (!IsRowNotEmpty(rows[lastNotEmptyRowIndex]))
            {
                lastNotEmptyRowIndex--;
            }
            return lastNotEmptyRowIndex;
        }

        private bool IsRowNotEmpty(TableRow row)
        {
            return row.Elements<TableCell>()
                .Any(c => !string.IsNullOrEmpty(GetCellText(c)));
        }

        private static string GetCellText(TableCell cell)
        {
            Paragraph? paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            if (paragraph is null)
            {
                return "";
            }

            Run? run = paragraph.Elements<Run>().FirstOrDefault();
            if (run is null)
            {
                return "";
            }

            Text? text = run.Elements<Text>().FirstOrDefault();
            if (text is null)
            {
                return "";
            }

            return text.InnerText;
        }
    }
}
