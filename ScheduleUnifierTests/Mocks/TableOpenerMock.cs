using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Parsing.TableOpeners;

namespace ScheduleUnifierTests.Mocks
{
    internal class TableOpenerMock : ITableOpener
    {
        public TableOpenerMock(ITable table, IFacultyAndSpecializationParser facultyAndSpecializationParser)
        {
            Table = table;
            FacultyAndSpecializationParser = facultyAndSpecializationParser;
        }

        public ITable Table { get; init; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; init; }
    }
}
