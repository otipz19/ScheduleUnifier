namespace ScheduleUnifier.Exceptions
{
    public class InvalidDirectoryPathException : Exception
    {
        public InvalidDirectoryPathException(string? path) : base($"Invalid directory path: {path}")
        {
        }
    }
}
