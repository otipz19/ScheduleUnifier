namespace ScheduleUnifier.Parsing.Exceptions
{
    public class EmptyTableException : InvalidTableFormatException
    {
        public EmptyTableException() : base("Empty table")
        {
        }
    }
}
