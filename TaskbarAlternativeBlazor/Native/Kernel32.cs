using System.Runtime.InteropServices;

namespace TaskbarAlternativeBlazor.Native;

internal static partial class Kernel32
{
    public static void RestartProcess()
    {
        string modulePath = Environment.ProcessPath!;

        StartupInfo si = new();
        si.cb = Marshal.SizeOf(si);
        ProcessInformation pi = new();

        // Spawn the new instance
        bool success = CreateProcessW(
            null,
            modulePath,
            IntPtr.Zero,
            IntPtr.Zero,
            false,
            0,
            IntPtr.Zero,
            null,
            ref si,
            out pi);

        if (success)
        {
            // Close handles to avoid leaks
            CloseHandle(pi.hThread);
            CloseHandle(pi.hProcess);

            // Exit the current process cleanly
            ExitProcess(0);
        }
        else
        {
            int error = Marshal.GetLastWin32Error();
            Console.WriteLine($"CreateProcess failed. Win32 Error: {error}");
        }
    }

    [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateProcessW(
        [MarshalAs(UnmanagedType.LPWStr)] string lpApplicationName,
        [MarshalAs(UnmanagedType.LPWStr)] string lpCommandLine,
        IntPtr lpProcessAttributes,
        IntPtr lpThreadAttributes,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        [MarshalAs(UnmanagedType.LPWStr)] string lpCurrentDirectory,
        ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial void ExitProcess(uint uExitCode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseHandle(IntPtr hObject);
}