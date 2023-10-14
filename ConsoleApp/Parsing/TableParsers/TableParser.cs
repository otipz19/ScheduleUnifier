using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Parsing.TableOpeners;

namespace ScheduleUnifier.Parsing.TableParsers
{
    public class TableParser : ITableParser
    {
        private readonly string[] headerValues = new[] { "день", "час", "дисципліна", "група", "тижні", "аудиторія" };

        private readonly ITable table;
        private readonly IFacultyAndSpecializationParser facultyAndSpecializationParser;
        private int lastNotEmptyRow;

        public TableParser(ITableOpener tableOpener)
        {
            table = tableOpener.Table;
            facultyAndSpecializationParser = tableOpener.FacultyAndSpecializationParser;
        }

        public ParsedTable Parse()
        {
            this.lastNotEmptyRow = table.GetLastNotEmptyRow();
            int headerRowIndex = FindHeaderRow();
            var parsedTable = new ParsedTable();
            (parsedTable.Faculty, parsedTable.Specializations) = facultyAndSpecializationParser.Parse();
            parsedTable.Rows = ParseRows(headerRowIndex);
            return parsedTable;
        }

        private List<ParsedRow> ParseRows(int headerRowIndex)
        {
            var parsedRows = new List<ParsedRow>();

            string? lastDay = null;
            string? lastTime = null;

            for (int row = headerRowIndex; row <= lastNotEmptyRow; row++)
            {
                if (IsNotEmptyCell(row, 0))
                {
                    lastDay = table[row, 0];
                }
                else if(lastDay is null)
                {
                    throw new MissingSavedDayException(row);
                }

                if (IsNotEmptyCell(row, 1))
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

            for (int col = 2; IsNotEmptyCell(row, col); col++)
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

        private bool IsNotEmptyCell(int row, int col)
        {
            return !string.IsNullOrWhiteSpace(table[row, col]);
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

            return startRow + 1;
        }

        private bool IsHeaderRow(int row)
        {
            return Enumerable.Range(0, 6)
                .Any(i => !string.IsNullOrWhiteSpace(table[row, i]) 
                    && headerValues[i].Contains(table[row, i], StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
