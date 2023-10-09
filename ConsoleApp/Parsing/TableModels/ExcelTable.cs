using OfficeOpenXml;

namespace ConsoleApp.Parsing.TableModels
{
    internal class ExcelTable : ITable
    {
        private readonly ExcelWorksheet table;

        public ExcelTable(ExcelWorksheet table)
        {
            this.table = table;
        }

        public string this[int row, int col] => table.Cells[row, col].Text;

        public int GetLastNotEmptyRow()
        {
            if (table.Dimension == null)
            {
                return 0;
            }
            var row = table.Dimension.End.Row;
            while (row >= 1)
            {
                var range = table.Cells[row, 1, row, table.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }
    }
}
