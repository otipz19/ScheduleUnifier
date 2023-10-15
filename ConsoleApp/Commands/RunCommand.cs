using ScheduleUnifier.Configuration;
using System.CommandLine;

namespace ScheduleUnifier.Commands
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", "Runs formating process")
        {
            this.SetHandler(() =>
            {
                var unifier = new Unifier(ConfigurationProvider.GetConfiguration());
                unifier.Run();
                Console.WriteLine($"Successfully processed {unifier.FilesProcessed} of {unifier.FilesTotal} files!");
            });
        }
    }
}
