namespace ConsoleApp.Parsing.Models
{
    internal class ParsedRow
    {
        public string? Day { get; set; }

        public string? Time { get; set; }

        public string? Discipline { get; set; }

        public string? Group { get; set; }

        public string? Weeks { get; set; }

        public string? Classroom { get; set; }

        public override string ToString()
        {
            return $"| {Day} | {Time} | {Discipline} | {Group} | {Weeks} | {Classroom} |";
        }
    }
}
