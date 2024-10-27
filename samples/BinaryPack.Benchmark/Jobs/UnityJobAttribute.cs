using System;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using Microsoft.Win32;

namespace BinaryPack.Benchmark.Jobs;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = true)]
public class UnityJobAttribute : MonoJobAttribute
{
    public UnityJobAttribute(string version)
        : base($"Unity {version}", GetUnityMonoExeFromVersion(version))
    {
    }

    private static string GetUnityMonoExeFromVersion(string version)
    {
        static void LogUnityNotOnMachine(string version)
        {
            ConsoleLogger.Default.WriteLineError($"Unity {version} was not found on this machine.");
        }

        // TODO: Support job on Mac/Linux platforms.
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ConsoleLogger.Default.WriteLineError("Unity jobs are not yet supported on operating systems other than Windows.");
            return "";
        }
        string? unityPath;
        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
        using (RegistryKey? unityInstaller = baseKey.OpenSubKey($@"SOFTWARE\Unity Technologies\Installer\Unity {version}"))
        {
            if (unityInstaller == null)
            {
                LogUnityNotOnMachine(version);
                return "";
            }
            unityPath = unityInstaller.GetValue("Location x64")?.ToString();
            if (unityPath == null)
            {
                LogUnityNotOnMachine(version);
                return "";
            }
        }
        string unityMonoExe = @$"{unityPath}\Editor\Data\MonoBleedingEdge\bin\mono.exe";
        if (!File.Exists(unityMonoExe))
        {
            LogUnityNotOnMachine(version);
            return "";
        }

        return unityMonoExe;
    }
}
