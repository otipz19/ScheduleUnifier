using ConsoleApp.Parsing.Exceptions;
using ConsoleApp.Parsing.TableModels;
using System.Text.RegularExpressions;

namespace ConsoleApp.Parsing.FacultyAndSpecializationParsers
{
    internal class ExcelFacultyAndSpecializationParser : IFacultyAndSpecializationParser
    {
        private readonly ITable table;

        public ExcelFacultyAndSpecializationParser(ITable table)
        {
            this.table = table;
        }

        public (string faculty, IEnumerable<string> specializations) Parse()
        {
            string? faculty = null;
            var specializations = new List<string>();

            int lastNotEmptyRow = table.GetLastNotEmptyRow();

            for (int row = 1; row < lastNotEmptyRow; row++)
            {
                for (int col = 1; !string.IsNullOrWhiteSpace(table[row, col]); col++)
                {
                    string cellText = table[row, col];
                    if (cellText.Contains("факультет", StringComparison.InvariantCultureIgnoreCase))
                    {
                        faculty = cellText.Trim();
                    }

                    else if (cellText.Contains("спеціальність", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //This regex pattern retrieves from string all substrings that contained inside either "" or «» quotes
                        specializations = Regex.Matches(cellText, "(?<=\"|«)[^\"«]+(?=\"|»)")
                                .Select(m => m.Value.Trim())
                                .ToList();
                    }
                }
            }

            if (faculty is null)
            {
                throw new NotFoundFacultyException();
            }

            if (!specializations.Any())
            {
                throw new NotFoundSpecializationsException();
            }

            return (faculty, specializations);
        }
    }
}
