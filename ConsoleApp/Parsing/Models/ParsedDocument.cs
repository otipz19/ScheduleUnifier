namespace ScheduleUnifier.Parsing.Models
{
    public class ParsedDocument
    {
        public string Faculty { get; set; } = default!;

        public IEnumerable<string> Specializations { get; set; } = new List<string>();

        public IEnumerable<ParsedRow> Rows { get; set; } = new List<ParsedRow>();
    }
}
