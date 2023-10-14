using ConsoleApp.Parsing.FacultyAndSpecializationParsers;
using ConsoleApp.Parsing.TableModels;

namespace ConsoleApp.Parsing.TableOpeners
{
    internal interface ITableOpener
    {
        public ITable Table { get; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; }
    }
}
