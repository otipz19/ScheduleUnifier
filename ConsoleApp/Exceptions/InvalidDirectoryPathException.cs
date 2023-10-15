namespace ScheduleUnifier.Exceptions
{
    public class InvalidDirectoryPathException : ConfigurationException
    {
        public InvalidDirectoryPathException(string? path) : base($"Invalid directory path: {path}")
        {
        }
    }
}
