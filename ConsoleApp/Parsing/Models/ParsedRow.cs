namespace ScheduleUnifier.Parsing.Models
{
    public record ParsedRow
    {
        public string Day { get; set; } = default!;

        public string Time { get; set; } = default!;

        public string Discipline { get; set; } = default!;

        public string Group { get; set; } = default!;

        public string Weeks { get; set; } = default!;

        public string Classroom { get; set; } = default!;

        public override string ToString()
        {
            return $"| {Day} | {Time} | {Discipline} | {Group} | {Weeks} | {Classroom} |";
        }
    }
}
