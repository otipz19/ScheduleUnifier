using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.TableModels;
using ScheduleUnifier.Serialization.Models;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    internal class ExcelFacultyAndSpecializationParser : IFacultyAndSpecializationParser
    {
        private readonly IEnumerable<ITable> tables;
        private readonly FacultyFinder facultyFinder = new FacultyFinder();
        private readonly SpecializationsFinder specializationsFinder = new SpecializationsFinder();

        private string? faculty = null;
        private List<string>? specializations = null;
        private bool hasFoundFaculty = false;
        private bool hasFoundSpecializations = false;

        public ExcelFacultyAndSpecializationParser(IEnumerable<ITable> tables)
        {
            this.tables = tables;
        }

        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            foreach (var table in tables)
            {
                if (hasFoundFaculty && hasFoundSpecializations)
                    break;
                ParseTable(table);
            }

            if (!hasFoundFaculty)
            {
                throw new NotFoundFacultyException();
            }

            if (!hasFoundSpecializations)
            {
                throw new NotFoundSpecializationsException();
            }

            return (faculty!, specializations!);
        }

        private void ParseTable(ITable table)
        {
            int lastNotEmptyRow = table.GetLastNotEmptyRow();

            for (int row = 0; row <= lastNotEmptyRow && !(hasFoundFaculty && hasFoundSpecializations); row++)
            {
                for (int col = 0; !string.IsNullOrWhiteSpace(table[row, col]); col++)
                {
                    string cellText = table[row, col];
                    if (!hasFoundFaculty)
                    {
                        hasFoundFaculty = facultyFinder.TryFind(cellText, out faculty);
                    }
                    if (!hasFoundSpecializations)
                    {
                        hasFoundSpecializations = specializationsFinder.TryFind(cellText, out specializations);
                    }
                }
            }
        }
    }
}
