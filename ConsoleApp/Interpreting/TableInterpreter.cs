using ConsoleApp.Interpreting.Models;
using ConsoleApp.Parsing.Models;
using System.Text.RegularExpressions;

namespace ConsoleApp.Interpreting
{
    internal class TableInterpreter
    {
        public IEnumerable<RecordModel> Interpret(ParsedTable parsedTable)
        {
            if (parsedTable.Specializations.Count() == 1)
            {
                return parsedTable.Rows
                    .Select(r => MapRowToRecord(r, parsedTable.Faculty, parsedTable.Specializations.First()))
                    .ToList();
            }
            else if (parsedTable.Specializations.Count() > 1)
            {
                return parsedTable.Rows
                    .SelectMany(r => InterpretSpecialization(parsedTable, r))
                    .ToList();
            }
            else
            {
                throw new Exception($"Invalid specialization count: {parsedTable.Specializations.Count()}");
            }
        }

        private IEnumerable<RecordModel> InterpretSpecialization(ParsedTable parsedTable, ParsedRow parsedRow)
        {
            var records = new List<RecordModel>();

            //Regex pattern to find substrings enclosed within parentheses ()
            IEnumerable<string> matchings = Regex.Matches(parsedRow.Discipline, "\\(([^)]+)\\)").Select(m => m.Value);
            foreach (string matching in matchings)
            {
                string[] aliases = matching
                    .Split('+', StringSplitOptions.TrimEntries)
                    .Select(a => a.Trim('(', ')', '.'))
                    .ToArray();

                foreach(string alias in aliases)
                {
                    string? specialization = parsedTable.Specializations
                        .FirstOrDefault(s => s.Contains(alias, StringComparison.InvariantCultureIgnoreCase));

                    if (specialization is not null)
                    {
                        //If there is specialization alias inside brackets, delete all brackets' content from original string
                        parsedRow.Discipline = parsedRow.Discipline.Replace(matching, "");
                        records.Add(MapRowToRecord(parsedRow, parsedTable.Faculty, specialization));
                    }
                }
            }

            return records;
        }

        private RecordModel MapRowToRecord(ParsedRow parsedRow, string faculty, string specialization)
        {
            return new RecordModel()
            {
                Faculty = faculty,
                Specialization = specialization,
                Day = parsedRow.Day,
                Time = parsedRow.Time,
                Discipline = parsedRow.Discipline,
                Group = FormatGroup(parsedRow.Group),
                Weeks = parsedRow.Weeks,
                Classroom = FormatClassroom(parsedRow.Classroom),
            };
        }

        private string FormatClassroom(string classroom)
        {
            if(classroom.Equals("д", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Дистанційно";
            }
            return classroom;
        }

        private string FormatGroup(string group)
        {
            if(group.Contains("лекція", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Лекція";
            }
            else
            {
                //Regex to find any number substring
                string? groupNum = Regex.Matches(group, "\\d+").Select(m => m.Value).FirstOrDefault();

                if(groupNum is null)
                {
                    throw new Exception("Invalid group format");
                }

                return groupNum;
            }
        }
    }
}
