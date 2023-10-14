using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    internal interface IFacultyAndSpecializationParser
    {
        public (string faculty, IEnumerable<string> specializations) Parse();
    }
}
