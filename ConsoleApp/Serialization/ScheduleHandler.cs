using ScheduleUnifier.Serialization.Models;

namespace ScheduleUnifier.Serialization
{
    public class ScheduleHandler
    {
        public Schedule Schedule { get; init; } = new Schedule();

        public void Serialize(string outputDir, string outputFile)
        {
            string json = Schedule.ToJsonString();

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            File.WriteAllText(outputFile, json);
        }
    }
}
