using NPOI.HWPF.UserModel;

namespace ScheduleUnifier.Parsing.TableModels
{
    internal class DocTable : ITable
    {
        private Table table;

        public DocTable(Table table)
        {
            this.table = table;
        }

        public string this[int row, int col]
        {
            get
            {
                try
                {
                    TableCell cell = table.GetRow(row).GetCell(col);
                    return cell.Text;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public int GetLastNotEmptyRow()
        {
            return table.NumRows;
        }
    }
}
