namespace ScheduleUnifier.Configuration
{
    public class ConfigurationModel
    {
        public string InputDirPath { get; set; } = default!;

        public string OutputDirPath { get; set; } = default!;

        public bool UseHttp { get; set; } = default!;

        public IEnumerable<string> Urls { get; set; } = default!;
    }
}
