using System.Text.RegularExpressions;

namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    public class SpecializationsFinder
    {
        public bool TryFind(string text, out List<string>? specializations)
        {
            if (text.Contains("спеціальність", StringComparison.InvariantCultureIgnoreCase))
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
