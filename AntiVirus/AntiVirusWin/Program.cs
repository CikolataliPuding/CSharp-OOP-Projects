using System;
using System.Windows.Forms;

namespace AntiVirusWin
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girişi.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}



