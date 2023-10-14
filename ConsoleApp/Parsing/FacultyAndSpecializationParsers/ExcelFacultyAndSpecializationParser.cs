using ScheduleUnifier.Parsing.Exceptions;
using ScheduleUnifier.Parsing.TableModels;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    internal class ExcelFacultyAndSpecializationParser : IFacultyAndSpecializationParser
    {
        private readonly ITable table;
        private readonly FacultyFinder facultyFinder = new FacultyFinder();
        private readonly SpecializationsFinder specializationsFinder = new SpecializationsFinder();

        public ExcelFacultyAndSpecializationParser(ITable table)
        {
            this.table = table;
        }

        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            string? faculty = null;
            var specializations = new List<string>();

            int lastNotEmptyRow = table.GetLastNotEmptyRow();

            for (int row = 0; row < lastNotEmptyRow && (faculty is null || specializations is null); row++)
            {
                for (int col = 0; !string.IsNullOrWhiteSpace(table[row, col]); col++)
                {
                    string cellText = table[row, col];
                    if (faculty is null)
                    {
                        facultyFinder.TryFind(cellText, out faculty);
                    }
                    if(specializations is null || !specializations.Any())
                    {
                        specializationsFinder.TryFind(cellText, out specializations);
                    }
                }
            }

            if (faculty is null)
            {
                throw new NotFoundFacultyException();
            }

            if (specializations is null || !specializations.Any())
            {
                throw new NotFoundSpecializationsException();
            }

            return (faculty, specializations);
        }
    }
}
