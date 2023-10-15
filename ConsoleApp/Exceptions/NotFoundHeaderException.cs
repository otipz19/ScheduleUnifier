namespace ScheduleUnifier.Exceptions
{
    public class NotFoundHeaderException : InvalidTableFormatException
    {
        public NotFoundHeaderException() : base("Table missing valid header!")
        {
        }
    }
}
