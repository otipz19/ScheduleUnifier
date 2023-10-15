namespace ScheduleUnifier.Configuration
{
    public class ConfigurationModel
    {
        public string? InputDirPath { get; set; }

        public string? OutputDirPath { get; set; }

        public bool UseHttp { get; set; }

        public IEnumerable<string>? Urls { get; set; }
    }
}
