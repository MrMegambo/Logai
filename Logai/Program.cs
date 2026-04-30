using System;
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
    }
}
