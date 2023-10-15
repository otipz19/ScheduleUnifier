namespace ScheduleUnifier.Exceptions
{
    public class InputFilesNotFoundException : ApplicationException
    {
        public InputFilesNotFoundException() : base("Not found files to parse")
        {
        }
    }
}
