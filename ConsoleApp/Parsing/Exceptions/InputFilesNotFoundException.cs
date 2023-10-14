namespace ScheduleUnifier.Parsing.Exceptions
{
    public class InputFilesNotFoundException : ApplicationException
    {
        public InputFilesNotFoundException() : base("Not found files to parse")
        {
        }
    }
}
