using TaskbarAlternativeBlazor.Taskbar.Config;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

internal sealed class ConfigProvider
{
    public Configuration GetConfiguration() => new(
        false,
        false,
        [
        new(
            true,
            [],
            "",
            "",
            "",
            new([], ["tab.clock"], []))
        ]);
}