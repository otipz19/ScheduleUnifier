using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.Models;
using OfficeOpenXml;

namespace ConsoleApp.Parsing
{
    internal abstract class TableParserBase
    {
        

        protected abstract void OpenTable(string filePath);
        protected abstract int FindHeaderRow();
        protected abstract bool IsNotEmptyCell(int row, int col);
        protected abstract void ParseFacultyAndSpecializations(int headerRowIndex, ParsedTable parsedTable);
        protected abstract ParsedRow? ParseRow(int row, string lastDay, string lastTime);
        protected abstract List<ParsedRow> ParseRows(int headerRowIndex);

        
    }
}