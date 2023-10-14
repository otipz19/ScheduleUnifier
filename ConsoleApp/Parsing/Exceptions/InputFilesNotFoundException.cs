namespace ScheduleUnifier.Parsing.Exceptions
{
    internal class InputFilesNotFoundException : ApplicationException
    {
        public InputFilesNotFoundException() : base("Not found files to parse")
        {
        }
    }
}
