namespace ScheduleUnifier.Parsing.TableModels
{
    public interface ITable
    {
        public string this[int row, int col] { get; }

        public int GetLastNotEmptyRow();
    }
}
