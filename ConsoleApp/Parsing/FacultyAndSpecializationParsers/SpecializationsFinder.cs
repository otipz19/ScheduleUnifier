using System.Text.RegularExpressions;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    public class SpecializationsFinder
    {
        public bool TryFind(string text, out List<string>? specializations)
        {
            var specWordRegex = new Regex(@"с\s*п\s*е\s*ц\s*і\s*а\s*л\s*ь\s*н\s*і\s*с\s*т", RegexOptions.IgnoreCase);
            if (specWordRegex.Matches(text).Any())
            {
                //This regex pattern retrieves from string all substrings that contained inside either "" or «» quotes
                specializations = Regex.Matches(text, "(?<=\"|«)[^\"«]+(?=\"|»)")
                        .Select(m => m.Value.Trim())
                        .ToList();
                return true;
            }

            specializations = null;
            return false;
        }
    }
}
