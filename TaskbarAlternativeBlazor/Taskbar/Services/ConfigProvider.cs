using TaskbarAlternativeBlazor.Taskbar.Config;
using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

internal sealed class ConfigProvider
{
    private readonly Configuration _default = new(
    false,
    false,
    new()
    {
        ["default"] = new(
        true,
        [],
        "bottom",
        "100%",
        "47",
        new([], [], []))
    });
    
    // public Configuration GetConfiguration() => new(
    //     false,
    //     false,
    //     [
    //     new(
    //         true,
    //         [],
    //         "",
    //         "",
    //         "",
    //         new([], ["tab.clock"], []))
    //     ]);

    public async Task<Configuration> GetConfigurationAsync()
    {
        var configFilePath = Path.Combine(AppContext.BaseDirectory, "config.yaml");

        if (!File.Exists(configFilePath))
        {
            var stream = File.Create(configFilePath);

            using StreamWriter writer = new(stream);
            await writer.WriteAsync(new Serializer().Serialize(_default));

            return _default;
        }

        var configStream = File.OpenRead(configFilePath);
        using StreamReader reader = new(configStream);

        return new Deserializer().Deserialize<Configuration>(reader);
    }
}