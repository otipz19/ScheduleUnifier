using ConsoleApp.Parsing.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace ConsoleApp.Parsing
{
    internal class ExcelTableParser
    {
        private ExcelWorksheet table = default!;

        public ParsedTable Parse(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage(fileInfo);
            this.table = package.Workbook.Worksheets[0];
            return ParseTable();
        }

        private ParsedTable ParseTable()
        {
            int headerRowIndex = FindHeaderRow();
            var parsedTable = new ParsedTable();
            ParseFacultyAndSpecializations(headerRowIndex, parsedTable);
            parsedTable.Rows = ParseRows(headerRowIndex);
            return parsedTable;
        }

        private void ParseFacultyAndSpecializations(int headerRowIndex, ParsedTable parsedTable)
        {
            for (int row = 1; row < headerRowIndex; row++)
            {
                for (int col = 1; IsNotEmptyCell(row, col); col++)
                {
                    string cellText = table.Cells[row, col].Text;
                    if (cellText.Contains("факультет", StringComparison.InvariantCultureIgnoreCase))
                    {
                        parsedTable.Faculty = cellText.Trim();
                    }

                    else if (cellText.Contains("спеціальність", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //This regex pattern retrieves from string all substrings that contained inside either "" or «» quotes
                        parsedTable.Specializations = Regex.Matches(cellText, "(?<=\"|«)[^\"«]+(?=\"|»)")
                                .Select(m => m.Value.Trim())
                                .ToList();
                    }
                }
            }
        }

        private List<ParsedRow> ParseRows(int headerRowIndex)
        {
            var rowModels = new List<ParsedRow>();
            int lastNotEmptyRow = GetLastNotEmptyRow();

            string lastDay = "";
            string lastTime = "";

            for (int row = headerRowIndex; row < lastNotEmptyRow; row++)
            {
                if (IsNotEmptyCell(row, 1))
                {
                    lastDay = table.Cells[row, 1].Text;
                }

                if(IsNotEmptyCell(row, 2))
                {
                    lastTime = table.Cells[row, 2].Text;
                }

                ParsedRow? parsedRow = ParseRow(row, lastDay, lastTime);

                if (parsedRow is not null)
                {
                    rowModels.Add(parsedRow);
                }
            }

            return rowModels;
        }

        private ParsedRow? ParseRow(int row, string lastDay, string lastTime)
        {
            var parsedValues = new List<string>();

            for (int col = 3; IsNotEmptyCell(row, col); col++)
            {
                parsedValues.Add(table.Cells[row, col].Text);
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
            return !string.IsNullOrWhiteSpace(table.Cells[row, col].Text);
        }

        private int FindHeaderRow()
        {
            int startRow = 1;
            while (!this.table.Cells[startRow, 1].Text
                .Equals("День", StringComparison.InvariantCultureIgnoreCase))
            {
                startRow++;
            }

            return startRow + 1;
        }

        private int GetLastNotEmptyRow()
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
