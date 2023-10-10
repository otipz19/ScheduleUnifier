using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing
{
    internal interface IFacultyAndSpecializationParser
    {
        public (string faculty, IEnumerable<string> specializations) Parse();
    }
}
