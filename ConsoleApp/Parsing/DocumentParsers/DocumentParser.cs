using ScheduleUnifier.Exceptions;
using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.Models;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Parsing.TableOpeners;
using ScheduleUnifier.Parsing.TableParsers;

namespace ScheduleUnifier.Parsing.DocumentParsers
{
    internal class DocumentParser : IDocumentParser
    {
        private readonly IEnumerable<ITable> tables;
        private readonly IFacultyAndSpecializationParser facultyAndSpecializationParser;

        public DocumentParser(IDocumentOpener documentOpener)
        {
            tables = documentOpener.Tables;
            facultyAndSpecializationParser = documentOpener.FacultyAndSpecializationParser;
        }

        public ParsedDocument Parse()
        {
            var parsedTable = new ParsedDocument();
            (parsedTable.Faculty, parsedTable.Specializations) = facultyAndSpecializationParser.Parse();

            var parsedRows = new List<ParsedRow>();
            bool foundTargetTables = false;
            foreach(var table in tables)
            {
                var tableParser = new TableParser(table);
                if (tableParser.IsTargetTable())
                {
                    foundTargetTables |= tableParser.IsTargetTable();
                    parsedRows.AddRange(tableParser.ParseRows());
                }
            }

            if(!foundTargetTables)
            {
                throw new NotFoundTargetTablesException();
            }

            parsedTable.Rows = parsedRows;
            return parsedTable;
        }
    }
}
