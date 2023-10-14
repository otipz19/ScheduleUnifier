using ScheduleUnifier.Parsing.Models;

namespace ScheduleUnifier.Parsing.TableParsers
{
    public interface ITableParser
    {
        ParsedTable Parse();
    }
}