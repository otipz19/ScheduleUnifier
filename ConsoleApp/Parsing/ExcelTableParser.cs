using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.Models;
using ConsoleApp.Parsing.TableModels;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace ConsoleApp.Parsing
{
    internal class ExcelTableParser : ITableParser
    {
        private ITable table = default!;

        public ParsedTable Parse(string filePath)
        {
            TryOpenTable(filePath);
            return ParseTable();
        }

        private void TryOpenTable(string filePath)
        {
            try
            {
                OpenTable(filePath);
            }
            catch (Exception ex)
            {
                throw new TableNotFoundException(filePath, ex);
            }
        }

        private void OpenTable(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var package = new ExcelPackage(fileInfo);
            ExcelWorksheet excelWorksheet = package.Workbook.Worksheets[0];
            this.table = new ExcelTable(excelWorksheet);
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
                    string cellText = table[row, col];
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
            var parsedRows = new List<ParsedRow>();
            int lastNotEmptyRow = table.GetLastNotEmptyRow();

            string lastDay = "";
            string lastTime = "";

            for (int row = headerRowIndex; row < lastNotEmptyRow; row++)
            {
                if (IsNotEmptyCell(row, 1))
                {
                    lastDay = table[row, 1];
                }

                if (IsNotEmptyCell(row, 2))
                {
                    lastTime = table[row, 2];
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

            for (int col = 3; IsNotEmptyCell(row, col); col++)
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
            int startRow = 1;
            while (!this.table[startRow, 1]
                .Equals("День", StringComparison.InvariantCultureIgnoreCase))
            {
                var debug = this.table[startRow, 1];
                startRow++;
            }

            return startRow + 1;
        }
    }
}
