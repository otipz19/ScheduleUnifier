namespace ScheduleUnifier.Exceptions
{
    public class EmptyTableException : InvalidTableFormatException
    {
        public EmptyTableException() : base("Empty table")
        {
        }
    }
}
