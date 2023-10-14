namespace ScheduleUnifier.Parsing.TableModels
{
    public interface ITable
    {
        public string this[int row, int col] { get; }

        public int GetLastNotEmptyRow();

        public bool IsNotEmptyCell(int row, int col)
        {
            return !string.IsNullOrWhiteSpace(this[row, col]);
        }
    }
}
