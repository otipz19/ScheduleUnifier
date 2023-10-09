using ConsoleApp.Parsing.Models;

namespace ConsoleApp.Parsing
{
    internal interface ITableParser
    {
        ParsedTable Parse(string filePath);
    }
}