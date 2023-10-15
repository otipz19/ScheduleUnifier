namespace ScheduleUnifier.Exceptions
{
    public abstract class InvalidTableFormatException : Exception
    {
        protected InvalidTableFormatException(string? message) : base($"Invalid table format: {message}")
        {
        }
    }
}
