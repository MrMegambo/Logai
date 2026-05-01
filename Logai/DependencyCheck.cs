using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Logai
{
    internal static class DependencyCheck
    {
        private static readonly LocalDependency[] RequiredLocalDependencies =
        {
            new LocalDependency("LibreHardwareMonitorLib.dll", "LibreHardwareMonitorLib", new Version(0, 9, 6, 0), null, true),
            new LocalDependency("BlackSharp.Core.dll", "BlackSharp.Core", new Version(1, 0, 7, 0), null, false),
            new LocalDependency("DiskInfoToolkit.dll", "DiskInfoToolkit", new Version(1, 1, 2, 0), null, false),
            new LocalDependency("HidSharp.dll", "HidSharp", new Version(2, 6, 4, 0), null, false),
            new LocalDependency("RAMSPDToolkit-NDD.dll", "RAMSPDToolkit-NDD", new Version(1, 4, 2, 0), null, false),
            new LocalDependency("System.Buffers.dll", "System.Buffers", new Version(4, 0, 5, 0), "cc7b13ffcd2ddd51", false),
            new LocalDependency("System.Memory.dll", "System.Memory", new Version(4, 0, 5, 0), "cc7b13ffcd2ddd51", false),
            new LocalDependency("System.Numerics.Vectors.dll", "System.Numerics.Vectors", new Version(4, 1, 6, 0), "b03f5f7f11d50a3a", false),
            new LocalDependency("System.Runtime.CompilerServices.Unsafe.dll", "System.Runtime.CompilerServices.Unsafe", new Version(6, 0, 3, 0), "b03f5f7f11d50a3a", false)
        };

        public static DependencyCheckResult ValidateApplicationDependencies()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            List<string> problems = new List<string>();

            foreach (LocalDependency dependency in RequiredLocalDependencies)
            {
                ValidateLocalDependency(appDirectory, dependency, problems);
            }

            if (problems.Count == 0)
            {
                ValidateCriticalTypes(problems);
            }

            return new DependencyCheckResult(appDirectory, problems);
        }

        public static string BuildFailureMessage(DependencyCheckResult result)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Logai cannot start because required runtime dependencies are missing or invalid.");
            message.AppendLine();
            message.AppendLine("Application folder:");
            message.AppendLine(result.ApplicationDirectory);
            message.AppendLine();
            message.AppendLine("Problems found:");

            foreach (string problem in result.Problems)
            {
                message.AppendLine("- " + problem);
            }

            message.AppendLine();
            message.AppendLine("The required DLL files must stay in the same folder as Logai.exe.");

            return message.ToString();
        }

        private static void ValidateLocalDependency(string appDirectory, LocalDependency dependency, List<string> problems)
        {
            string dependencyPath = Path.Combine(appDirectory, dependency.FileName);

            if (!File.Exists(dependencyPath))
            {
                problems.Add(dependency.FileName + " is missing from the application folder.");
                return;
            }

            if (dependency.Requires64BitProcess && !Environment.Is64BitProcess)
            {
                problems.Add(dependency.FileName + " requires Logai to run as a 64-bit process.");
            }

            try
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(dependencyPath);

                if (!string.Equals(assemblyName.Name, dependency.AssemblyName, StringComparison.OrdinalIgnoreCase))
                {
                    problems.Add(dependency.FileName + " has assembly identity '" + assemblyName.Name + "' instead of '" + dependency.AssemblyName + "'.");
                    return;
                }

                if (assemblyName.Version == null || assemblyName.Version.CompareTo(dependency.MinimumVersion) < 0)
                {
                    problems.Add(dependency.FileName + " is version " + FormatVersion(assemblyName.Version) + "; expected " + dependency.MinimumVersion + " or newer.");
                }

                if (!string.IsNullOrEmpty(dependency.PublicKeyToken))
                {
                    string actualToken = FormatPublicKeyToken(assemblyName.GetPublicKeyToken());
                    if (!string.Equals(actualToken, dependency.PublicKeyToken, StringComparison.OrdinalIgnoreCase))
                    {
                        problems.Add(dependency.FileName + " has public key token '" + actualToken + "' instead of '" + dependency.PublicKeyToken + "'.");
                    }
                }
            }
            catch (BadImageFormatException ex)
            {
                problems.Add(dependency.FileName + " is not a valid managed DLL for this process: " + ex.Message);
            }
            catch (FileLoadException ex)
            {
                problems.Add(dependency.FileName + " exists but could not be loaded: " + ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                problems.Add(dependency.FileName + " references a missing assembly: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                problems.Add(dependency.FileName + " cannot be read due to file permissions: " + ex.Message);
            }
            catch (IOException ex)
            {
                problems.Add(dependency.FileName + " cannot be inspected: " + ex.Message);
            }
        }

        private static void ValidateCriticalTypes(List<string> problems)
        {
            try
            {
                Type.GetType("LibreHardwareMonitor.Hardware.Computer, LibreHardwareMonitorLib", true);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is FileLoadException || ex is BadImageFormatException || ex is TypeLoadException)
            {
                problems.Add("LibreHardwareMonitorLib could not expose the required hardware sensor type: " + ex.Message);
            }
        }

        private static string FormatVersion(Version version)
        {
            return version == null ? "unknown" : version.ToString();
        }

        private static string FormatPublicKeyToken(byte[] token)
        {
            if (token == null || token.Length == 0)
            {
                return "none";
            }

            StringBuilder builder = new StringBuilder(token.Length * 2);
            foreach (byte value in token)
            {
                builder.Append(value.ToString("x2"));
            }

            return builder.ToString();
        }

        private sealed class LocalDependency
        {
            public LocalDependency(string fileName, string assemblyName, Version minimumVersion, string publicKeyToken, bool requires64BitProcess)
            {
                FileName = fileName;
                AssemblyName = assemblyName;
                MinimumVersion = minimumVersion;
                PublicKeyToken = publicKeyToken;
                Requires64BitProcess = requires64BitProcess;
            }

            public string FileName { get; }

            public string AssemblyName { get; }

            public Version MinimumVersion { get; }

            public string PublicKeyToken { get; }

            public bool Requires64BitProcess { get; }
        }
    }

    internal sealed class DependencyCheckResult
    {
        public DependencyCheckResult(string applicationDirectory, IReadOnlyList<string> problems)
        {
            ApplicationDirectory = applicationDirectory;
            Problems = problems;
        }

        public string ApplicationDirectory { get; }

        public IReadOnlyList<string> Problems { get; }

        public bool IsHealthy
        {
            get { return Problems.Count == 0; }
        }
    }
}
