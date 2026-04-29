using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SwingMusic
{
    internal static class ConfigStore
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SwingMusicApp",
            "config.json");

        public static string LoadUrl()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    return string.Empty;
                }

                var config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(ConfigPath));
                return config != null && config.Url != null ? config.Url : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SaveUrl(string url)
        {
            var directory = Path.GetDirectoryName(ConfigPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var config = new AppConfig { Url = url };
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config));
        }

        private sealed class AppConfig
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }
    }
}
