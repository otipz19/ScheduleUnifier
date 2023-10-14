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

        static async Task Main(string[] args)
        {
            SetupRootCommand();
            await rootCommand.InvokeAsync(args);
            var unifier = new Unifier(inputDirPath: inputDir, outputDirPath: outputDir, isHttp: isHttp, urls: urls);
            unifier.Run();
        }

        private static void SetupRootCommand()
        {
            rootCommand.AddAlias("unif");
            var inputDirOption = SetupInputDirOption();
            var outputDirOption = SetupOutputDirOption();
            var httpOption = SetupHttpOption();
            var urlsOption = SetupUrlsOption();
            rootCommand.SetHandler((inputDir, outputDir, isHttp, urls) =>
            {
                Program.inputDir = inputDir.FullName;
                Program.outputDir = outputDir.FullName;
                Program.isHttp = isHttp;
                Program.urls = urls;
            },
            inputDirOption, outputDirOption, httpOption, urlsOption);
        }

        private static Option<IEnumerable<string>> SetupUrlsOption()
        {
            var urlsOption = new Option<IEnumerable<string>>(
                            name: "--urls",
                            description: "URLs to be used for loading files, when HTTP loading is enabled");
            urlsOption.AddAlias("-u");
            rootCommand.Add(urlsOption);
            return urlsOption;
        }

        private static Option<bool> SetupHttpOption()
        {
            var httpOption = new Option<bool>(
                            name: "--http",
                            description: "Flag to determine whether files should be loaded via HTTP");
            httpOption.AddAlias("-h");
            rootCommand.Add(httpOption);
            return httpOption;
        }

        private static Option<DirectoryInfo> SetupOutputDirOption()
        {
            var outputDirOption = new Option<DirectoryInfo>(
                            name: "--output",
                            description: "Absolute or relative path to output directory");
            outputDirOption.AddAlias("-o");
            rootCommand.Add(outputDirOption);
            return outputDirOption; 
        }

        private static Option<DirectoryInfo> SetupInputDirOption()
        {
            var inputDirOption = new Option<DirectoryInfo>(
                            name: "--input",
                            description: "Absolute or relative path to input directory");
            inputDirOption.AddAlias("-i");
            rootCommand.Add(inputDirOption);
            return inputDirOption;
        }
    }
}