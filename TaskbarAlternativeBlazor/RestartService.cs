using TaskbarAlternativeBlazor.Native;
using TaskbarAlternativeBlazor.Widgets;

namespace TaskbarAlternativeBlazor;

internal sealed class RestartService : IRestartService
{
    public void Restart() => Kernel32.RestartProcess();
}