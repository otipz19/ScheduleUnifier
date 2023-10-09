//using ConsoleApp.Parsing.Exceptions;
//using ConsoleApp.Parsing.Models;
//using DocumentFormat.OpenXml.Drawing;
//using DocumentFormat.OpenXml.Packaging;

//namespace ConsoleApp.Parsing
//{
//    internal class DocTableParser : TableParserBase, ITableParser
//    {
//        protected override void OpenTable(string filePath)
//        {
//            var doc = WordprocessingDocument.Open(filePath, isEditable: false);
//            table = doc.MainDocumentPart!.Document.Body!.Elements<Table>().First();
//        }

//        private ParsedTable ParseTable()
//        {
//            int headerRowIndex = FindHeaderRow();
//            var parsedTable = new ParsedTable();
//            ParseFacultyAndSpecializations(headerRowIndex, parsedTable);
//            parsedTable.Rows = ParseRows(headerRowIndex);
//            return parsedTable;
//        }

//        protected override void OpenTable(string filePath)
//        {
//            throw new NotImplementedException();
//        }

//        protected override int FindHeaderRow()
//        {
//            throw new NotImplementedException();
//        }

//        protected override bool IsNotEmptyCell(int row, int col)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void ParseFacultyAndSpecializations(int headerRowIndex, ParsedTable parsedTable)
//        {
//            throw new NotImplementedException();
//        }

//        protected override ParsedRow? ParseRow(int row, string lastDay, string lastTime)
//        {
//            throw new NotImplementedException();
//        }

//        protected override List<ParsedRow> ParseRows(int headerRowIndex)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
