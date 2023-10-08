using ConsoleApp.Interpreting;
using ConsoleApp.Interpreting.Models;
using ConsoleApp.Parsing;
using ConsoleApp.Parsing.Models;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new ExcelTableParser();
            string filePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "fen.xlsx");
            ParsedTable parsedTable = parser.Parse(filePath);
            var interpreter = new TableInterpreter();
            IEnumerable<RecordModel> records = interpreter.Interpret(parsedTable);
            Console.WriteLine();
        }
    }
}