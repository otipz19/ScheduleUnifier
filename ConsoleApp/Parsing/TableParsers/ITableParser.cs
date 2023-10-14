using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifier.Parsing.TableParsers
{
    public interface ITableParser
    {
        public bool IsTargetTable();

        public IEnumerable<ParsedRow> ParseRows();
    }
}