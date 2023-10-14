namespace ScheduleUnifier.Parsing.TableModels
{
    internal interface ITable
    {
        public string this[int row, int col] { get; }

        public int GetLastNotEmptyRow();
    }
}
