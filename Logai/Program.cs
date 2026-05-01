using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace Logai
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!EnsureRunningAsAdministrator())
            {
                return;
            }

            DependencyCheckResult dependencyCheck = DependencyCheck.ValidateApplicationDependencies();
            if (!dependencyCheck.IsHealthy)
            {
                MessageBox.Show(
                    DependencyCheck.BuildFailureMessage(dependencyCheck),
                    "Logai dependency check failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            RunApplication();
        }

        private static void RunApplication()
        {
            Application.Run(new Form1());
        }

        private static bool EnsureRunningAsAdministrator()
        {
            if (IsRunningAsAdministrator())
            {
                return true;
            }

            if (TryRestartAsAdministrator(out string elevationFailureMessage))
            {
                Environment.ExitCode = 0;
                return false;
            }

            string message = elevationFailureMessage
                + Environment.NewLine
                + Environment.NewLine
                + "Logai requires administrator privileges to read hardware telemetry and system information accurately. The application will now close.";

            MessageBox.Show(message, "Administrator privileges required", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Environment.ExitCode = 1;
            return false;
        }

        private static bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
                return currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static bool TryRestartAsAdministrator(out string failureMessage)
        {
            failureMessage = string.Empty;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = BuildCommandLineArguments(Environment.GetCommandLineArgs().Skip(1)),
                    WorkingDirectory = Application.StartupPath,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process elevatedProcess = Process.Start(startInfo);
                if (elevatedProcess == null)
                {
                    failureMessage = "Windows did not start the elevated Logai process.";
                    return false;
                }

                return true;
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                failureMessage = "Administrator approval was not granted.";
                return false;
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException || ex is SecurityException)
            {
                failureMessage = "Logai could not request administrator privileges: " + ex.Message;
                return false;
            }
        }

        private static string BuildCommandLineArguments(IEnumerable<string> arguments)
        {
            return string.Join(" ", arguments.Select(QuoteCommandLineArgument));
        }

        private static string QuoteCommandLineArgument(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                return "\"\"";
            }

            bool requiresQuotes = argument.Any(char.IsWhiteSpace) || argument.Contains("\"");
            if (!requiresQuotes)
            {
                return argument;
            }

            StringBuilder quotedArgument = new StringBuilder();
            quotedArgument.Append('"');

            int backslashCount = 0;
            foreach (char character in argument)
            {
                if (character == '\\')
                {
                    backslashCount++;
                    continue;
                }

                if (character == '"')
                {
                    quotedArgument.Append('\\', (backslashCount * 2) + 1);
                    quotedArgument.Append('"');
                    backslashCount = 0;
                    continue;
                }

                quotedArgument.Append('\\', backslashCount);
                backslashCount = 0;
                quotedArgument.Append(character);
            }

            quotedArgument.Append('\\', backslashCount * 2);
            quotedArgument.Append('"');

            return quotedArgument.ToString();
        }
    }
}
