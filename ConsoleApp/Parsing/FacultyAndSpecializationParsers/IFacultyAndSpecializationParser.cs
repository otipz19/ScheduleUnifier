using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing.FacultyAndSpecializationParsers
{
    internal interface IFacultyAndSpecializationParser
    {
        public (string faculty, IEnumerable<string> specializations) Parse();
    }
}
