namespace ScheduleUnifier.Exceptions
{
    public abstract class ConfigurationException : Exception
    {
        public ConfigurationException(string? message) : base(message)
        {
        }
    }
}
