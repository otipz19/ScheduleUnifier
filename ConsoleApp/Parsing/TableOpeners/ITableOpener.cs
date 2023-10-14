using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    public interface ITableOpener
    {
        public ITable Table { get; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; }
    }
}
