namespace ScheduleUnifier.Exceptions
{
    public class UrlsNotProvidedException : Exception
    {
        public UrlsNotProvidedException() : base("Urls must be provided for HTTP mode")
        {
        }
    }
}
