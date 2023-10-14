using ScheduleUnifier.Interpreting.Models;
using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifier.Interpreting
{
    public interface ITableInterpreter
    {
        IEnumerable<RecordModel> Interpret(ParsedTable parsedTable);
    }
}