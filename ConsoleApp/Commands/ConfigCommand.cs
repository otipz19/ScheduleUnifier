using ScheduleUnifier.Configuration;
using ScheduleUnifier.Exceptions;
using System.CommandLine;

namespace ScheduleUnifier.Commands
{
    public class ConfigCommand : Command
    {
        public ConfigCommand() : base("config", "Sets up configuration parameters")
        {
            var inputDirOption = SetupInputDirOption();
            var outputDirOption = SetupOutputDirOption();
            var httpOption = SetupHttpOption();
            var urlsOption = SetupUrlsOption();
            this.SetHandler((inputDir, outputDir, isHttp, urls) =>
            {
                try
                {
                    ConfigurationProvider.SetParameters(inputDir?.FullName ?? null, outputDir?.FullName ?? null, isHttp, urls);
                    Console.WriteLine("Successfully set up configuration parameters!");
                }
                catch (ConfigurationException ex)
                {
                    Console.WriteLine($"Wrong parameters provided: {ex}");
                }
            },
            inputDirOption, outputDirOption, httpOption, urlsOption);
        }

        private Option<IEnumerable<string>?> SetupUrlsOption()
        {
            var urlsOption = new Option<IEnumerable<string>?>(
                            name: "--urls",
                            description: "URLs to be used for loading files, when HTTP loading is enabled");
            urlsOption.AddAlias("-u");
            urlsOption.AllowMultipleArgumentsPerToken = true;
            this.Add(urlsOption);
            return urlsOption;
        }

        private Option<bool?> SetupHttpOption()
        {
            var httpOption = new Option<bool?>(
                            name: "--http",
                            description: "Flag to determine whether files should be loaded via HTTP");
            httpOption.AddAlias("-h");
            this.Add(httpOption);
            return httpOption;
        }

        private Option<DirectoryInfo?> SetupOutputDirOption()
        {
            var outputDirOption = new Option<DirectoryInfo?>(
                            name: "--output",
                            description: "Absolute or relative path to output directory");
            outputDirOption.AddAlias("-o");
            this.Add(outputDirOption);
            return outputDirOption;
        }

        private Option<DirectoryInfo?> SetupInputDirOption()
        {
            var inputDirOption = new Option<DirectoryInfo?>(
                            name: "--input",
                            description: "Absolute or relative path to input directory");
            inputDirOption.AddAlias("-i");
            this.Add(inputDirOption);
            return inputDirOption;
        }
    }
}
