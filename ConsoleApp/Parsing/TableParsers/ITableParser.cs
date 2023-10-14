using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifier.Parsing.TableParsers
{
    internal interface ITableParser
    {
        ParsedTable Parse();
    }
}