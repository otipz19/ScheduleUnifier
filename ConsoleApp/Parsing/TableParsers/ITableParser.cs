using ConsoleApp.Parsing.Models;

namespace ConsoleApp.Parsing.TableParsers
{
    internal interface ITableParser
    {
        ParsedTable Parse();
    }
}