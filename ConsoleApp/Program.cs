using System.CommandLine;

namespace ScheduleUnifier
{
    internal static class Program
    {
        private static readonly RootCommand rootCommand = new RootCommand("App for formating NaUKMA schedules into JSON format");

        private static string? inputDir;
        private static string? outputDir;
        private static bool isHttp;
        private static IEnumerable<string>? urls;

        static void Main(string[] args)
        {
            SetupCommandLine();
            var unifier = new Unifier(inputDirPath: inputDir, outputDirPath: outputDir, isHttp: isHttp, urls: urls);
            unifier.Run();
        }

        private static void SetupCommandLine()
        {
            SetupInputDirOption();
            SetupOutputDirOption();
            SetupHttpOption();
            SetupUrlsOption();
        }

        private static void SetupUrlsOption()
        {
            var urlsOption = new Option<IEnumerable<string>>(
                            name: "--urls",
                            description: "URLs to be used for loading files, when HTTP loading is enabled");
            rootCommand.AddOption(urlsOption);
            rootCommand.SetHandler((urls) =>
            {
                Program.urls = urls;
            },
            urlsOption);
        }

        private static void SetupHttpOption()
        {
            var isHttpOption = new Option<bool>(
                            name: "--http",
                            description: "Flag to determine whether files should be loaded via HTTP");
            rootCommand.AddOption(isHttpOption);
            rootCommand.SetHandler((isHttp) =>
            {
                Program.isHttp = isHttp;
            },
            isHttpOption);
        }

        private static void SetupOutputDirOption()
        {
            var outputDirOption = new Option<DirectoryInfo>(
                            name: "--output",
                            description: "Absolute or relative path to output directory");
            rootCommand.AddOption(outputDirOption);
            rootCommand.SetHandler((dir) =>
            {
                outputDir = dir.FullName;
            },
            outputDirOption);
        }

        private static void SetupInputDirOption()
        {
            var inputDirOption = new Option<DirectoryInfo>(
                            name: "--input",
                            description: "Absolute or relative path to input directory");
            rootCommand.AddOption(inputDirOption);
            rootCommand.SetHandler((dir) =>
            {
                inputDir = dir.FullName;
            },
            inputDirOption);
        }
    }
}