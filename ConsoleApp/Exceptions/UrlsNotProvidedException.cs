namespace ScheduleUnifier.Exceptions
{
    public class UrlsNotProvidedException : ConfigurationException
    {
        public UrlsNotProvidedException() : base("Urls must be provided for HTTP mode")
        {
        }
    }
}
