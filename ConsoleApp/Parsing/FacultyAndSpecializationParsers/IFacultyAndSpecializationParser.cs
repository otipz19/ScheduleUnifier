namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    public interface IFacultyAndSpecializationParser
    {
        public (string faculty, IEnumerable<string> specializations) Parse();
    }
}
