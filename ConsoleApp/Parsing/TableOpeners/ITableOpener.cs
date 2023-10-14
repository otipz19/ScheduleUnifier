using ScheduleUnifier.Parsing.FacultyAndSpecializationParsers;
using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.TableOpeners
{
    internal interface ITableOpener
    {
        public ITable Table { get; }

        public IFacultyAndSpecializationParser FacultyAndSpecializationParser { get; }
    }
}
