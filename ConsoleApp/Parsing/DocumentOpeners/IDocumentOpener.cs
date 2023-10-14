using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    public interface IDocumentOpener
    {
        public IEnumerable<ITable> Tables { get; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; }
    }
}
