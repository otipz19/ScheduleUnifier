using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;

namespace ScheduleUnifierTests.Mocks
{
    internal class FacultyAndSpecializationParserMock : IFacultyAndSpecializationParser
    {
        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            return ("Faculty", Enumerable.Range(1, 3).Select(i => $"spec{i}"));
        }
    }
}
