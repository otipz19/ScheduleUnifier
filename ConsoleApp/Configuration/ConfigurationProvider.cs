﻿using ScheduleUnifier.Exceptions;
using System.Text.Json;

namespace ScheduleUnifier.Configuration
{
    public static class ConfigurationProvider
    {
        private const string ConfigFileName = "config.json";

        private static readonly string configFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

        private static readonly ConfigurationModel defaultConfig = new ConfigurationModel()
        {
            InputDirPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "input"),
            OutputDirPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output"),
            UseHttp = true,
            Urls = new string[]
            {
                "https://my.ukma.edu.ua/files/schedule/2023/1/1475/1.docx",
                "https://my.ukma.edu.ua/files/schedule/2023/1/1472/3.xlsx",
                "https://my.ukma.edu.ua/files/schedule/2023/1/1483/3.xlsx",
                "https://my.ukma.edu.ua/files/schedule/2023/1/1562/1.docx",
            },
        };

        public static bool ConfigurationExists => File.Exists(configFilePath);

        public static ConfigurationModel GetConfiguration()
        {
            SetupConfiguration();
            string json = File.ReadAllText(configFilePath);
            var config = TryDeserializeConfig(configFilePath, json);
            return ValidateConfig(config);
        }

        public static void SetupConfiguration()
        {
            if (!ConfigurationExists)
            {
                PersistConfig(configFilePath, defaultConfig);
            }
        }

        public static void SetInputDir(string inputDir)
        {
            SetParameter((config) => config.InputDirPath = inputDir);
        }

        public static void SetOutputDir(string outputDir)
        {
            SetParameter((config) => config.OutputDirPath = outputDir);
        }

        public static void SetUseHttp(bool useHttp)
        {
            SetParameter((config) => config.UseHttp = useHttp);
        }

        public static void SetUrls(IEnumerable<string> urls)
        {
            SetParameter((config) => config.Urls = urls);
        }

        private static void SetParameter(Action<ConfigurationModel> action)
        {
            var config = GetConfiguration();
            action(config);
            PersistConfig(configFilePath, config);
        }

        private static void ValidateHttpConfig(ConfigurationModel config)
        {
            if (config.UseHttp && (config.Urls is null || !config.Urls.Any()))
            {
                throw new UrlsNotProvidedException();
            }
        }

        private static string ValidatePath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidDirectoryPathException(path);
            }
            return MakePathRooted(path);
        }

        private static string MakePathRooted(string path)
        {
            if (!Path.IsPathRooted(path))
            {
               return Path.Join(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return path;
        }

        private static ConfigurationModel TryDeserializeConfig(string configFilePath, string json)
        {
            try
            {
                ConfigurationModel? config = JsonSerializer.Deserialize<ConfigurationModel>(json);
                if (config is null)
                {
                    return HandleConfigReadFail(configFilePath);
                }
                return config;
            }
            catch
            {
                return HandleConfigReadFail(configFilePath);
            }
        }

        private static ConfigurationModel HandleConfigReadFail(string configFilePath)
        {
            Console.WriteLine("""
                    ConfigurationProvider couldn't read configuration file.
                    Applying and persisting default configuration.
                    """);
            PersistConfig(configFilePath, defaultConfig);
            return defaultConfig;
        }

        private static void PersistConfig(string configFilePath, ConfigurationModel config)
        {
            config = ValidateConfig(config);
            File.Create(configFilePath);
            string defaultConfigJson = JsonSerializer.Serialize(defaultConfig);
            File.WriteAllText(configFilePath, defaultConfigJson);
        }

        private static ConfigurationModel ValidateConfig(ConfigurationModel config)
        {
            ValidateHttpConfig(config);
            config.InputDirPath = ValidatePath(config.InputDirPath);
            config.OutputDirPath = ValidatePath(config.OutputDirPath);
            return config;
        }
    }
}
