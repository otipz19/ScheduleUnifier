using ScheduleUnifier.Commands;
using ScheduleUnifier.Exceptions;
using System.CommandLine;

namespace ScheduleUnifier
{
    internal static class Program
    {
        private static readonly RootCommand rootCommand = new RootCommand("App for formating NaUKMA schedules into JSON format");

        static void Main(string[] args)
        {
            rootCommand.AddCommand(new SetupCommand());
            rootCommand.AddCommand(new ConfigCommand());
            rootCommand.AddCommand(new RunCommand());
            rootCommand.Invoke(args);
        }
    }
}