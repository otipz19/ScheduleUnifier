using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing
{
    internal interface ITableOpener
    {
        public ITable Table { get; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; }
    }
}
