using ScheduleUnifier.Configuration;
using System.CommandLine;

namespace ScheduleUnifier.Commands
{
    internal class SetupCommand : Command
    {
        public SetupCommand() : base("setup", "Sets up application environment")
        {
            this.SetHandler(() =>
            {
                var config = ConfigurationProvider.GetConfiguration();

                if (!Directory.Exists(config.InputDirPath))
                {
                    Directory.CreateDirectory(config.InputDirPath);
                }

                if(!Directory.Exists(config.OutputDirPath))
                {
                    Directory.CreateDirectory(config.OutputDirPath);
                }

                Console.WriteLine("Successfully set up application environment!");
            });
        }
    }
}
