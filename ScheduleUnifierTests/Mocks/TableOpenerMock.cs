using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Parsing.TableOpeners;

namespace ScheduleUnifierTests.Mocks
{
    internal class TableOpenerMock : IDocumentOpener
    {
        public TableOpenerMock(ITable table, IFacultyAndSpecializationParser facultyAndSpecializationParser)
        {
            Tables = new ITable[] { table };
            FacultyAndSpecializationParser = facultyAndSpecializationParser;
        }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; init; }

        public IEnumerable<ITable> Tables { get; init; }
    }
}
