using DocumentFormat.OpenXml.Wordprocessing;

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
                //Just to conduct with 1-based indexing from EPPlus
                row--;
                col--;

                var rows = table.Elements<TableRow>().ToList();
                if (row >= rows.Count() - 1)
                {
                    return string.Empty;
                }

                var cells = rows.ElementAt(row).Elements<TableCell>();
                if(col >= cells.Count() - 1)
                {
                    return string.Empty;
                }

                TableCell cell = cells.ElementAt(col);
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
            var paragraphs = cell.Elements<Paragraph>();
            if (!paragraphs.Any())
            {
                return string.Empty;
            }

            var runs = paragraphs.SelectMany(p => p.Elements<Run>());
            if (!runs.Any())
            {
                return string.Empty;
            }

            var texts = runs.SelectMany(r => r.Elements<Text>().Select(t => t.Text)).ToArray();
            if (!texts.Any())
            {
                return string.Empty;
            }

            return string.Join(string.Empty, texts);
        }
    }
}
