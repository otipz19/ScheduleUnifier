using ScheduleUnifier.Exceptions;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.TableParsers
{
    public class TableParser : ITableParser
    {
        private readonly string[] headerValues = new[] { "день", "час", "дисципліна", "група", "тижні", "аудиторія" };
        private readonly ITable table;

        private int lastNotEmptyRow;
        private int headerRowIndex;
        private bool? isTarget;

        public TableParser(ITable table)
        {
            this.table = table;
        }

        public bool IsTargetTable()
        {
            if (isTarget.HasValue)
                return isTarget.Value;

            try
            {
                this.lastNotEmptyRow = table.GetLastNotEmptyRow();
                headerRowIndex = FindHeaderRow();
                isTarget = true;
                return isTarget.Value;
            }
            catch (NotFoundHeaderException)
            {
                isTarget = false;
                return isTarget.Value;
            }
            catch(EmptyTableException)
            {
                isTarget = false;
                return isTarget.Value;
            }
        }

        public IEnumerable<ParsedRow> ParseRows()
        {
            if (!isTarget.HasValue)
            {
                IsTargetTable();
            }

            if(!isTarget!.Value)
            {
                throw new NotFoundHeaderException();
            }

            return ParseRows(headerRowIndex);
        }

        private List<ParsedRow> ParseRows(int headerRowIndex)
        {
            var parsedRows = new List<ParsedRow>();

            string? lastDay = null;
            string? lastTime = null;

            for (int row = headerRowIndex + 1; row <= lastNotEmptyRow; row++)
            {
                if (table.IsNotEmptyCell(row, 0))
                {
                    lastDay = table[row, 0];
                }
                else if(lastDay is null)
                {
                    throw new MissingSavedDayException(row);
                }

                if (table.IsNotEmptyCell(row, 1))
                {
                    lastTime = table[row, 1];
                }
                else if (lastTime is null)
                {
                    throw new MissingSavedTimeException(row);
                }

                ParsedRow? parsedRow = ParseRow(row, lastDay, lastTime);

                if (parsedRow is not null)
                {
                    parsedRows.Add(parsedRow);
                }
            }

            return parsedRows;
        }

        private ParsedRow? ParseRow(int row, string lastDay, string lastTime)
        {
            var parsedValues = new List<string>();

            for (int col = 2; table.IsNotEmptyCell(row, col); col++)
            {
                parsedValues.Add(table[row, col]);
            }

            if (parsedValues.Count != 4)
            {
                return null;
            }

            return new ParsedRow()
            {
                Day = lastDay,
                Time = lastTime,
                Discipline = parsedValues[0],
                Group = parsedValues[1],
                Weeks = parsedValues[2],
                Classroom = parsedValues[3],
            };
        }

        private int FindHeaderRow()
        {
            int startRow = 0;
            while (!IsHeaderRow(startRow))
            {
                startRow++;
                if(startRow > this.lastNotEmptyRow)
                {
                    throw new NotFoundHeaderException();
                }
            }

            return startRow;
        }

        private bool IsHeaderRow(int row)
        {
            return Enumerable.Range(0, 6)
                .Any(i => !string.IsNullOrWhiteSpace(table[row, i]) 
                    && headerValues[i].Contains(table[row, i], StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
