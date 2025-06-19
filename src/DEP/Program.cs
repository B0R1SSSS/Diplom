using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DEP
{
    internal static class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Initialize application folders and settings
                if (!Directory.Exists(OtherMethods.ExamsPath))
                {
                    Directory.CreateDirectory(OtherMethods.ExamsPath);
                }
                if (!Directory.Exists(OtherMethods.TestsPath))
                {
                    Directory.CreateDirectory(OtherMethods.TestsPath);
                }

                // Start with the main form
                Application.Run(new StartingForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при запуске приложения: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
