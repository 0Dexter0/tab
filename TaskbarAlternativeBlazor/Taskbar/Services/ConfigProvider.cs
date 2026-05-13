using TaskbarAlternativeBlazor.Taskbar.Config;
using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

public sealed class ConfigProvider
{
    private static readonly string ConfigFilePath = Path.Combine(AppContext.BaseDirectory, "config.yaml");

    private readonly Configuration _default = new()
            {
                WatchStyles = false,
                WatchConfig =  false,
                Bars = [
                    new()
                    {
                        Enabled = true,
                        Name = "default",
                        Height = "47",
                        Width = "100%",
                        Position = "bottom",
                        Centered = true,
                        Screens = [],
                        Widgets = new()
                        {
                            Left = [],
                            Center = ["clock"],
                            Right = []
                        }
                    }]
            };

    public async Task<Configuration> GetConfigurationAsync()
    {
        await EnsureExistsAsync();
        return Load();
    }

    private Task EnsureExistsAsync() =>
        File.Exists(ConfigFilePath)
            ? Task.CompletedTask
            : File.WriteAllTextAsync(ConfigFilePath, new Serializer().Serialize(_default));

    private Configuration Load()
    {
        using var reader = File.OpenText(ConfigFilePath);
        return new Deserializer().Deserialize<Configuration>(reader);
    }
}