namespace ConsoleApp.Parsing.Models
{
    internal class ParsedTable
    {
        public string Faculty { get; set; } = default!;

        public IEnumerable<string> Specializations { get; set; } = new List<string>();

        public IEnumerable<ParsedRow> Rows { get; set; } = new List<ParsedRow>();
    }
}
