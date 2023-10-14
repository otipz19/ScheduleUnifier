namespace ScheduleUnifier.Parsing.FacultyAndSpecializationParsers
{
    public class FacultyFinder
    {
        public bool TryFind(string text, out string? faculty)
        {
            if (text.Contains("факультет", StringComparison.InvariantCultureIgnoreCase)
                && !text.Contains("декан", StringComparison.InvariantCultureIgnoreCase))
            {
                faculty = text.Trim();
                return true;
            }

            faculty = null;
            return false;
        }
    }
}
