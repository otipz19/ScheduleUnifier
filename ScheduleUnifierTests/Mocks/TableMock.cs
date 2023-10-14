using ScheduleUnifier.Parsing.TableModels;

internal class TableMock : ITable
{
    private readonly string[,] cells;

    public TableMock(string[,] cells)
    {
        this.cells = cells;
    }

    public string this[int row, int col]
    {
        get
        {
            return cells[row, col];
        }
    }

    public int GetLastNotEmptyRow()
    {
        return cells.GetLength(0) - 1;
    }
}